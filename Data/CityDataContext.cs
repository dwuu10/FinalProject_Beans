using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CityModel;

    public class CityDataContext : DbContext
    {
        public CityDataContext (DbContextOptions<CityDataContext> options)
            : base(options)
        {
        }

        public DbSet<CityModel.City> City { get; set; } = default!;
    }
