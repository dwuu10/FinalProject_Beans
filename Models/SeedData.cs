using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CityModel;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Final_Project;

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
                // Look for any CityModels.
                if (context.City.Any())
                {
                    return;   // DB has been seeded
                }

                string[] cityList = ["Seattle", "Stockhom", "Rome", "Manila"];

                context.City.AddRange(
                    new City
                    {
                        Name = cityList[0],
                        Temperature = OpenWeatherMapAPI.Weather(cityList[0]),
                        Humidity = OpenWeatherMapAPI.Humidity(cityList[0]),
                        BluePrice = CityData.CalculateBlueBeanPrice(OpenWeatherMapAPI.Weather(cityList[0])),
                        RedPrice = CityData.CalculateRedBeanPrice(OpenWeatherMapAPI.Weather(cityList[0])),
                        YellowPrice = CityData.CalculateYellowBeanPrice(OpenWeatherMapAPI.Humidity(cityList[0])),
                        GreenPrice = CityData.CalculateGreenBeanPrice(OpenWeatherMapAPI.Humidity(cityList[0]))
                    },

                    new City
                    {
                        Name = cityList[1],
                        Temperature = OpenWeatherMapAPI.Weather(cityList[1]),
                        Humidity = OpenWeatherMapAPI.Humidity(cityList[1]),
                        BluePrice = CityData.CalculateBlueBeanPrice(OpenWeatherMapAPI.Weather(cityList[1])),
                        RedPrice = CityData.CalculateRedBeanPrice(OpenWeatherMapAPI.Weather(cityList[1])),
                        YellowPrice = CityData.CalculateYellowBeanPrice(OpenWeatherMapAPI.Humidity(cityList[1])),
                        GreenPrice = CityData.CalculateGreenBeanPrice(OpenWeatherMapAPI.Humidity(cityList[1]))
                    },

                    new City
                    {
                        Name = cityList[2],
                        Temperature = OpenWeatherMapAPI.Weather(cityList[2]),
                        Humidity = OpenWeatherMapAPI.Humidity(cityList[2]),
                        BluePrice = CityData.CalculateBlueBeanPrice(OpenWeatherMapAPI.Weather(cityList[2])),
                        RedPrice = CityData.CalculateRedBeanPrice(OpenWeatherMapAPI.Weather(cityList[2])),
                        YellowPrice = CityData.CalculateYellowBeanPrice(OpenWeatherMapAPI.Humidity(cityList[2])),
                        GreenPrice = CityData.CalculateGreenBeanPrice(OpenWeatherMapAPI.Humidity(cityList[2]))
                    },

                    new City
                    {
                        Name = cityList[3],
                        Temperature = OpenWeatherMapAPI.Weather(cityList[0]),
                        Humidity = OpenWeatherMapAPI.Humidity(cityList[0]),
                        BluePrice = CityData.CalculateBlueBeanPrice(OpenWeatherMapAPI.Weather(cityList[3])),
                        RedPrice = CityData.CalculateRedBeanPrice(OpenWeatherMapAPI.Weather(cityList[3])),
                        YellowPrice = CityData.CalculateYellowBeanPrice(OpenWeatherMapAPI.Humidity(cityList[3])),
                        GreenPrice = CityData.CalculateGreenBeanPrice(OpenWeatherMapAPI.Humidity(cityList[3]))
                    }
                );
                context.SaveChanges();
            }
        }
    }
}