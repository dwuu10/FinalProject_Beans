using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace Final_Project.Controllers
{
    public class GameWindowController : Controller
    {
        // 
        // GET: /HelloWorld/

        public IActionResult Index()
        {
            //Game game = new Game(1000, 10, new string[] {"Seattle", "Stockholm", "Rome", "Manila"});

            return View(); //$"starting cash: {game.startingCash}";
        }

        public IActionResult GameLoop()
        {
            //Game game = new Game(1000, 10, new string[] {"Seattle", "Stockholm", "Rome", "Manila"});
            /*
            string[] cities = new string[] { "Seattle", "Stockholm", "Rome", "Manila" };
            int[] humidity = new int[cities.Length];
            int[] temperature = new int[cities.Length];
            //CityData[] citiesData = new CityData[cities.Length];
            int iterator = 0;
            foreach (string city in cities)
            {
                int cityHumidity = OpenWeatherMapAPI.Humidity(city);
                int cityTemperature = OpenWeatherMapAPI.Weather(city);
                humidity[iterator] = cityHumidity;
                temperature[iterator] = cityTemperature;
                iterator++;
            }

            ViewData["CityNames"] = cities;
            ViewData["CityHumidity"] = humidity;
            ViewData["CityTemperature"] = temperature;
            ViewData["ListLength"] = cities.Length;
            */
            

            int startingCash = 1000;

            ViewData["StartCash"] = startingCash;
            ViewData["Cash"] = startingCash;
            ViewData["Turn"] = 0;
            ViewData["MaxTurns"] = 10;
            ViewData["Blue"] = 0;
            ViewData["Red"] = 0;
            ViewData["Yellow"] = 0;
            ViewData["Green"] = 0;


            return View(); //$"starting cash: {game.startingCash}";
        }
    }
}