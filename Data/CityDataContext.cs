
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Final_Project.Models;

    public class CityDataContext : DbContext
    {
        public CityDataContext (DbContextOptions<CityDataContext> options)
            : base(options)
        {
        }

        public DbSet<Final_Project.Models.City> City { get; set; } = default!;
    }
    
