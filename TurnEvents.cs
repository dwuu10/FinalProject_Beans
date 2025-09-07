using System;

namespace Final_Project_Beans;

public class TurnEvents
{
    public static CityData[] UpdateCityWeather(CityData[] cities)
    {
        CityData[] updatedCities = new CityData[cities.Length];
        int newArrayIterator = 0;
        foreach (CityData city in cities)
        {
            var cityName = city.cityName;
            var newTemp = OpenWeatherMapAPI.Weather(cityName);
            var newHumid = OpenWeatherMapAPI.Humidity(cityName);
            CityData updatedCity = new CityData(cityName, newHumid, newTemp);
            updatedCities[newArrayIterator] = updatedCity;
            newArrayIterator++;
        }
        return updatedCities;
    }

    public static void TurnEventHandler(CityData[] cities)
    {
        var rng = new Random();
        var eventDiceRoll = rng.Next(5); // 2/6 chance of event, subject to change
        var cityNum = rng.Next(3);
        var cityToModify = cities[cityNum];
        if (eventDiceRoll <= 2)
        {
            var eventRoll = rng.Next(5);
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

    static void DroughtEvent(CityData city)
    {
        city.greenBeanPrice = city.greenBeanPrice * 2;
        city.yellowBeanPrice = city.yellowBeanPrice / 2;
        Console.WriteLine($"{city.cityName} suffers from a drought! Green Beans double in value, Yellow Beans have half their normal value");
    }

    static void FloodEvent(CityData city)
    {
        city.greenBeanPrice = city.greenBeanPrice / 2;
        city.yellowBeanPrice = city.yellowBeanPrice * 2;
        Console.WriteLine($"{city.cityName} suffers from a flood! Yellow Beans double in value, Green Beans have half their normal value");
    }

    static void HeatWaveEvent(CityData city)
    {
        city.blueBeanPrice = city.blueBeanPrice * 2;
        city.redBeanPrice = city.redBeanPrice / 2;
        Console.WriteLine($"{city.cityName} suffers from a drought! Blue Beans double in value, Red Beans have half their normal value");
    }

    static void BlizzardEvent(CityData city)
    {
        city.blueBeanPrice = city.blueBeanPrice / 2;
        city.redBeanPrice = city.redBeanPrice * 2;
        Console.WriteLine($"{city.cityName} suffers from a drought! Red Beans double in value, Blue Beans have half their normal value");
    }

    static void BlightEvent(CityData city)
    {
        string beanName = "";
        var rng = new Random();
        var beanSelection = rng.Next(3);
        var modifier = rng.Next(3) + 2;
        switch (beanSelection)
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

    static void FadEvent(CityData city)
    {
        string beanName = "";
        var rng = new Random();
        var beanSelection = rng.Next(3);
        var modifier = rng.Next(3) + 2;
        switch (beanSelection)
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