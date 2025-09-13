
using Final_Project;
using Final_Project.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CityModel.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CityDataContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<CityDataContext>>()))
            {
                // Look for any movies.
                if (context.City.Any())
                {
                    return;   // DB has been seeded
                }

                context.City.AddRange(
                    new City
                    {
                        Name = "Seattle",
                        BlueBeanPrice = CityData.CalculateBlueBeanPrice(OpenWeatherMapAPI.Weather("Seattle")),
                        RedBeanPrice = CityData.CalculateRedBeanPrice(OpenWeatherMapAPI.Weather("Seattle")),
                        YellowBeanPrice = CityData.CalculateYellowBeanPrice(OpenWeatherMapAPI.Humidity("Seattle")),
                        GreenBeanPrice = CityData.CalculateGreenBeanPrice(OpenWeatherMapAPI.Humidity("Seattle")),
                    },

                    new City
                    {
                        Name = "Stockholm",
                        BlueBeanPrice = CityData.CalculateBlueBeanPrice(OpenWeatherMapAPI.Weather("Stockholm")),
                        RedBeanPrice = CityData.CalculateRedBeanPrice(OpenWeatherMapAPI.Weather("Stockholm")),
                        YellowBeanPrice = CityData.CalculateYellowBeanPrice(OpenWeatherMapAPI.Humidity("Stockholm")),
                        GreenBeanPrice = CityData.CalculateGreenBeanPrice(OpenWeatherMapAPI.Humidity("Stockholm")),
                    },

                    new City
                    {
                        Name = "Rome",
                        BlueBeanPrice = CityData.CalculateBlueBeanPrice(OpenWeatherMapAPI.Weather("Rome")),
                        RedBeanPrice = CityData.CalculateRedBeanPrice(OpenWeatherMapAPI.Weather("Rome")),
                        YellowBeanPrice = CityData.CalculateYellowBeanPrice(OpenWeatherMapAPI.Humidity("Rome")),
                        GreenBeanPrice = CityData.CalculateGreenBeanPrice(OpenWeatherMapAPI.Humidity("Rome")),
                    },

                    new City
                    {
                        Name = "Manila",
                        BlueBeanPrice = CityData.CalculateBlueBeanPrice(OpenWeatherMapAPI.Weather("Manila")),
                        RedBeanPrice = CityData.CalculateRedBeanPrice(OpenWeatherMapAPI.Weather("Manila")),
                        YellowBeanPrice = CityData.CalculateYellowBeanPrice(OpenWeatherMapAPI.Humidity("Manila")),
                        GreenBeanPrice = CityData.CalculateGreenBeanPrice(OpenWeatherMapAPI.Humidity("Manila")),
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
