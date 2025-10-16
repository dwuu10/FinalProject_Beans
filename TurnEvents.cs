using System;

namespace Final_Project_Beans;
// contains functions for handling turn events such as random events and price updates
// random event modifiers stack on top of price calculated by real-life weather
public class TurnEvents
{
    // updates all city weather data
    public static CityData[] UpdateCityWeather(CityData[] cities)
    {
        CityData[] updatedCities = new CityData[cities.Length];
        int newArrayIterator = 0;
        foreach (CityData city in cities)
        {
            // get temperature and humidity data using current city name
            var cityName = city.cityName;
            var newTemp = OpenWeatherMapAPI.Weather(cityName);
            var newHumid = OpenWeatherMapAPI.Humidity(cityName);
            // create new city object and add it to updated cities list
            CityData updatedCity = new CityData(cityName, newHumid, newTemp);
            updatedCities[newArrayIterator] = updatedCity;
            newArrayIterator++;
        }
        return updatedCities;
    }

    // handles random events
    public static void TurnEventHandler(CityData[] cities)
    {
        var rng = new Random();
        var eventDiceRoll = rng.Next(5); // 2/6 chance of event, subject to change
        var cityNum = rng.Next(3);
        var cityToModify = cities[cityNum]; // randomly choose which city to apply the event to
        if (eventDiceRoll <= 2)
        {
            var eventRoll = rng.Next(5); // roll for the event
            switch (eventRoll)
            {
                case 0:
                    DroughtEvent(cityToModify);
                    break;
                case 1:
                    FloodEvent(cityToModify);
                    break;
                case 2:
                    HeatWaveEvent(cityToModify);
                    break;
                case 3:
                    BlizzardEvent(cityToModify);
                    break;
                case 4:
                    BlightEvent(cityToModify);
                    break;
                case 5:
                    FadEvent(cityToModify);
                    break;
            }

        }
    }

    // drought event: doubles green bean value, halves yellow bean value
    static void DroughtEvent(CityData city)
    {
        city.greenBeanPrice = city.greenBeanPrice * 2;
        city.yellowBeanPrice = city.yellowBeanPrice / 2;
        Console.WriteLine($"{city.cityName} suffers from a sudden drought! Green Beans double in value, Yellow Beans have half their normal value");
    }

    // flood event: halves green bean value, doubles yellow bean value
    static void FloodEvent(CityData city)
    {
        city.greenBeanPrice = city.greenBeanPrice / 2;
        city.yellowBeanPrice = city.yellowBeanPrice * 2;
        Console.WriteLine($"{city.cityName} suffers from a sudden flood! Yellow Beans double in value, Green Beans have half their normal value");
    }

    // heatwave event: doubles blue bean value, halves red bean value
    static void HeatWaveEvent(CityData city)
    {
        city.blueBeanPrice = city.blueBeanPrice * 2;
        city.redBeanPrice = city.redBeanPrice / 2;
        Console.WriteLine($"{city.cityName} suffers from a sudden heat wave! Blue Beans double in value, Red Beans have half their normal value");
    }

    // blizzard event: halves blue bean value, doubles red bean value
    static void BlizzardEvent(CityData city)
    {
        city.blueBeanPrice = city.blueBeanPrice / 2;
        city.redBeanPrice = city.redBeanPrice * 2;
        Console.WriteLine($"{city.cityName} suffers from a sudden blizzard! Red Beans double in value, Blue Beans have half their normal value");
    }

    // blight event: a random bean will have its value multiplied anywhere from 2 to 5 times
    static void BlightEvent(CityData city)
    {
        string beanName = ""; // bean name string used in message
        var rng = new Random();
        var beanSelection = rng.Next(3); // select bean to be modified
        var modifier = rng.Next(3) + 2; // select random modifier
        switch (beanSelection) // modify selected bean
        {
            case 0:
                beanName = "Blue Beans";
                city.blueBeanPrice = city.blueBeanPrice * modifier;
                break;
            case 1:
                beanName = "Red Beans";
                city.redBeanPrice = city.redBeanPrice * modifier;
                break;
            case 2:
                beanName = "Green Beans";
                city.greenBeanPrice = city.greenBeanPrice * modifier;
                break;
            case 3:
                beanName = "Yellow Beans";
                city.yellowBeanPrice = city.yellowBeanPrice * modifier;
                break;
        }
        Console.WriteLine($"A devastating blight has struck {city.cityName}! The value of {beanName} increases {modifier} times!");
    }

    // fad event: a random bean will have it's value multiplied anywhere from 2 to 5 times
    static void FadEvent(CityData city)
    {
        string beanName = ""; // bean name string used in message
        var rng = new Random();
        var beanSelection = rng.Next(3); // select bean to be modified
        var modifier = rng.Next(3) + 2; // select random modifier
        switch (beanSelection) // modify selected bean
        {
            case 0:
                beanName = "Blue Beans";
                city.blueBeanPrice = city.blueBeanPrice * modifier;
                break;
            case 1:
                beanName = "Red Beans";
                city.redBeanPrice = city.redBeanPrice * modifier;
                break;
            case 2:
                beanName = "Green Beans";
                city.greenBeanPrice = city.greenBeanPrice * modifier;
                break;
            case 3:
                beanName = "Yellow Beans";
                city.yellowBeanPrice = city.yellowBeanPrice * modifier;
                break;
        }
        Console.WriteLine($"{beanName} have suddenly become all the rage in {city.cityName} thanks to a social media trend! The value of {beanName} increases {modifier} times!");
    }
}