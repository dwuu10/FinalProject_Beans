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

        //TODO: cant pass complex objects, but can pass arrays, probably convert to passing multiple arrays, 1 per bean type + cities
        public IActionResult GameLoop(int StartCash, int TurnLimit, int Cash, int Turn, int Blue, int Red, int Yellow, int Green)
        {
            //Game game = new Game(1000, 10, new string[] {"Seattle", "Stockholm", "Rome", "Manila"});
            /*
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
            var cities = new string[] { "Seattle", "Stockholm", "Rome", "Manila" };

            var beanInventory = new int[] { Blue, Red, Yellow, Green };

            Game gameData = new Game(StartCash, TurnLimit, cities, Cash, Turn, Blue, Red, Yellow, Green);

            TurnEvents.UpdateCityWeather(gameData.cityList);
            string eventMessage = TurnEvents.TurnEventHandler(gameData.cityList);

            ViewData["Event"] = eventMessage;

            int[] bluePrices = new int[cities.Length];
            int[] redPrices = new int[cities.Length];
            int[] yellowPrices = new int[cities.Length];
            int[] greenPrices = new int[cities.Length];

            int iterator = 0;
            foreach (CityData city in gameData.cityList)
            {
                bluePrices[iterator] = city.blueBeanPrice;
                redPrices[iterator] = city.redBeanPrice;
                yellowPrices[iterator] = city.yellowBeanPrice;
                greenPrices[iterator] = city.greenBeanPrice;
                iterator++;
            }

            ViewData["Cash"] = Cash;
            ViewData["Turn"] = Turn;

            ViewData["StartCash"] = StartCash;
            ViewData["MaxTurns"] = TurnLimit;
            ViewData["Inventory"] = beanInventory;
            ViewData["Cities"] = cities;
            ViewData["BluePrices"] = bluePrices;
            ViewData["RedPrices"] = redPrices;
            ViewData["YellowPrices"] = yellowPrices;
            ViewData["GreenPrices"] = greenPrices;

            return View(); //$"starting cash: {game.startingCash}";
        }

        public IActionResult GameEnd(int StartCash, int TurnLimit, int Cash)
        {
            return View();
        }
    }
}