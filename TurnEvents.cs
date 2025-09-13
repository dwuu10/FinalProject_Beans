using System;

namespace Final_Project;

public class BeanModifier
{
    public BeanModifier(string name, float blueMod, float redMod, float yellowMod, float greenMod, string eventString)
    {
        cityName = name;
        blueModifier = blueMod;
        redModifier = redMod;
        yellowModifier = yellowMod;
        greenModifier = greenMod;
        eventMessage = eventString;
    }
    public string cityName;
    public float blueModifier;
    public float redModifier;
    public float yellowModifier;
    public float greenModifier;
    public string eventMessage;

}

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

    public static string TurnEventHandler(CityData[] cities)
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
                    return DroughtEvent(cityToModify);
                    break;
                case 1:
                    return FloodEvent(cityToModify);
                    break;
                case 2:
                    return HeatWaveEvent(cityToModify);
                    break;
                case 3:
                    return BlizzardEvent(cityToModify);
                    break;
                case 4:
                    return BlightEvent(cityToModify);
                    break;
                case 5:
                    return FadEvent(cityToModify);
                    break;
                default:
                    return "an invalid event?";
            }

        }
        return "no unusual events have occured";
    }

    static string DroughtEvent(CityData city)
    {
        city.greenBeanPrice = city.greenBeanPrice * 2;
        city.yellowBeanPrice = city.yellowBeanPrice / 2;
        return ($"{city.cityName} suffers from a sudden drought! Green Beans double in value, Yellow Beans have half their normal value");
    }

    static string FloodEvent(CityData city)
    {
        city.greenBeanPrice = city.greenBeanPrice / 2;
        city.yellowBeanPrice = city.yellowBeanPrice * 2;
        return ($"{city.cityName} suffers from a sudden flood! Yellow Beans double in value, Green Beans have half their normal value");
    }

    static string HeatWaveEvent(CityData city)
    {
        city.blueBeanPrice = city.blueBeanPrice * 2;
        city.redBeanPrice = city.redBeanPrice / 2;
        return ($"{city.cityName} suffers from a sudden heat wave! Blue Beans double in value, Red Beans have half their normal value");
    }

    static string BlizzardEvent(CityData city)
    {
        city.blueBeanPrice = city.blueBeanPrice / 2;
        city.redBeanPrice = city.redBeanPrice * 2;
        return ($"{city.cityName} suffers from a sudden blizzard! Red Beans double in value, Blue Beans have half their normal value");
    }

    static string BlightEvent(CityData city)
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
        return ($"A devastating blight has struck {city.cityName}! The value of {beanName} increases {modifier} times!");
    }

    static string FadEvent(CityData city)
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
        return ($"{beanName} have suddenly become all the rage in {city.cityName} thanks to a social media trend! The value of {beanName} increases {modifier} times!");
    }

    //new methods
    /*
    public static string citySelection(string[] cities)
    {
        /*
        float testF = 0.5f;
        int testInt = 2;
        int inttofloat = (int)(testInt * testF);


    var rng = new Random();
    var cityNum = rng.Next(cities.Length - 1);
        return cities[cityNum];
    }

    public static BeanModifier eventSelection(string[] cities)
    {
        var cityName = citySelection(cities);
        string beanName = "";
        string eventMessage = "";
        float blueMod = 1;
        float redMod = 1;
        float yellowMod = 1;
        float greenMod = 1;

        var rng = new Random();
        var eventNum = rng.Next(5);
        var beanNum = rng.Next(3);
        var modifier = rng.Next(3) + 2;

        switch (beanNum)
        {
            case 0:
                beanName = "Blue";
                break;
            case 1:
                beanName = "Red";
                break;
            case 2:
                beanName = "Yellow";
                break;
            case 3:
                beanName = "Green";
                break;

        }

        switch (eventNum)
        {
            case 0:
                eventMessage = $"{cityName} suffers from a sudden drought! Green Beans double in value, Yellow Beans have half their normal value";
                greenMod = 2;
                yellowMod = 0.5f;
                break;
            case 1:
                eventMessage = $"{cityName} suffers from a sudden flood! Yellow Beans double in value, Green Beans have half their normal value";
                yellowMod = 2;
                greenMod = 0.5f;
                break;
            case 2:
                eventMessage = $"{cityName} suffers from a sudden heat wave! Blue Beans double in value, Red Beans have half their normal value";
                blueMod = 2;
                redMod = 0.5f;
                break;
            case 3:
                eventMessage = $"{cityName} suffers from a sudden blizzard! Red Beans double in value, Blue Beans have half their normal value";
                redMod = 2;
                blueMod = 0.5f;
                break;
            case 4:
                eventMessage = $"A devastating blight has struck {cityName}! The value of {beanName} increases {modifier} times!";
                switch (beanName)
                {
                    case "Blue":
                        blueMod = modifier;
                        break;
                    case "Red":
                        redMod = modifier;
                        break;
                    case "Yellow":
                        yellowMod = modifier;
                        break;
                    case "Green":
                        greenMod = modifier;
                        break;
                }
                break;
            case 5:
                eventMessage = $"{beanName} have suddenly become all the rage in {cityName} thanks to a social media trend! The value of {beanName} increases {modifier} times!";
                switch (beanName)
                {
                    case "Blue":
                        blueMod = modifier;
                        break;
                    case "Red":
                        redMod = modifier;
                        break;
                    case "Yellow":
                        yellowMod = modifier;
                        break;
                    case "Green":
                        greenMod = modifier;
                        break;
                }
                break;
        }

        return new BeanModifier(cityName, blueMod, redMod, yellowMod, greenMod, eventMessage);
    }
    */
}   