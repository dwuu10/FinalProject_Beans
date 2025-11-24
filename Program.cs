using Final_Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CityModel.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CityDataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CityDataContext")));

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<CityDataContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("CityDataContext")));
}
else
{
    builder.Services.AddDbContext<CityDataContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ProductionCityDataContext")));
}


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    SeedData.Initialize(services);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
