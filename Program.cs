
using System.Runtime.InteropServices;

namespace Final_Project_Beans
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
            */


            /*
            var city1 = new int[] { OpenWeatherMapAPI.Weather("Seattle"), OpenWeatherMapAPI.Humidity("Seattle"), 150, 50, 100, 100 };
            var city2 = new int[] { OpenWeatherMapAPI.Weather("Stockholm"), OpenWeatherMapAPI.Humidity("Stockholm"), 100, 150, 50, 100 };
            var city3 = new int[] { OpenWeatherMapAPI.Weather("Rome"), OpenWeatherMapAPI.Humidity("Rome"), 100, 100, 150, 50 };
            var city4 = new int[] { OpenWeatherMapAPI.Weather("Manila"), OpenWeatherMapAPI.Humidity("Manila"), 50, 100, 100, 150 };
            var currentturn = 0;
            do
            {

            } while (currentturn <= turns);
            */

            //Console.WriteLine($"Seattle weather: {OpenWeatherMapAPI.Weather("Seattle")}; humidity: {OpenWeatherMapAPI.Humidity("Seattle")}");

            Game.StartGame();
        }
    }
}