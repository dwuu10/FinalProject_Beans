using System;
using System.Transactions;

namespace Final_Project;

public class Game
{
    // initialize new game
    public Game(int startCash, int maxTurns, string[] cityNames)
    {
        //set cash and turns
        startingCash = startCash;
        currentCash = startingCash;
        turnLimit = maxTurns;
        currentTurn = 0;

        //set bean bean amount
        blueBeans = 0;
        redBeans = 0;
        yellowBeans = 0;
        greenBeans = 0;

        //populate citylist
        var cities = new CityData[cityNames.Length];
        int iterator = 0;
        foreach (string name in cityNames)
        {
            var newTemp = OpenWeatherMapAPI.Weather(name);
            var newHumid = OpenWeatherMapAPI.Humidity(name);
            var newCity = new CityData(name, newHumid, newTemp);
            cities[iterator] = newCity;
            iterator++;
        }
        cityList = cities;
    }

    public Game(int startCash, int maxTurns, string[] cityNames, int cash, int turn, int blue, int red, int yellow, int green)
    {
        //set cash and turns
        startingCash = startCash;
        currentCash = cash;
        turnLimit = maxTurns;
        currentTurn = turn;

        //set bean bean amount
        blueBeans = blue;
        redBeans = red;
        yellowBeans = yellow;
        greenBeans = green;

        //populate citylist
        var cities = new CityData[cityNames.Length];
        int iterator = 0;
        foreach (string name in cityNames)
        {
            var newTemp = OpenWeatherMapAPI.Weather(name);
            var newHumid = OpenWeatherMapAPI.Humidity(name);
            var newCity = new CityData(name, newHumid, newTemp);
            cities[iterator] = newCity;
            iterator++;
        }
        cityList = cities;
    }

    public bool exitGameFlag = false;
    public CityData[] cityList;
    public int turnLimit;
    public int startingCash;
    public int currentTurn;
    public int currentCash;
    public int blueBeans;
    public int redBeans;
    public int yellowBeans;
    public int greenBeans;

    // start new game
    public static void NewGame()
    {
        var cityStrings = new string[] { "Seattle", "Stockholm", "Rome", "Manila" };
        int x = 0;
        int defaultCash = 1000;
        int defaultTurns = 10;

        Console.WriteLine("enter the amount of cash you would to start with, entering a non-number will default to 1000");
        var customCashInput = Console.ReadLine();
        var customCash = defaultCash;
        if (int.TryParse(customCashInput, out x))
        {
            customCash = int.Parse(customCashInput);
        }

        Console.WriteLine("enter the amount of turns you would like to play to, entering a non-number will default to 10");
        var customTurnsInput = Console.ReadLine();
        var customTurns = defaultTurns;
        if (int.TryParse(customTurnsInput, out x))
        {
            customTurns = int.Parse(customTurnsInput);
        }

        var gameObj = new Game(customCash, customTurns, cityStrings);
        gameObj.TurnLoop(gameObj);
    }

    private void TurnLoop(Game gameData)
    {
        if (gameData.exitGameFlag == true || currentTurn > turnLimit)
        {
            GameEnd(gameData);
            return;
        }
        else
        {
            var currentCityData = TurnEvents.UpdateCityWeather(gameData.cityList);
            Console.WriteLine($"--BEGINNING TURN {gameData.currentTurn}--");
            Console.WriteLine("--CITY DATA UPDATED--");
            gameData.cityList = currentCityData;
            TurnEvents.TurnEventHandler(currentCityData);
            gameData.PrintCityData();
            gameData.PrintPlayerData();
            gameData.PlayerActions(gameData);
            gameData.currentTurn++;
            TurnLoop(gameData);
        }
    }

    private void PrintCityData()
    {
        var cities = cityList;
        foreach (CityData city in cities)
        {
            Console.WriteLine($"--{city.cityName}:");
            Console.WriteLine($"Blue Bean Price: {city.blueBeanPrice}");
            Console.WriteLine($"Red Bean Price: {city.redBeanPrice}");
            Console.WriteLine($"Yellow Bean Price: {city.yellowBeanPrice}");
            Console.WriteLine($"Green Bean Price: {city.greenBeanPrice}");
        }
        Console.WriteLine("------------------");
    }

    private void PrintPlayerData()
    {
        Console.WriteLine($"Turns: {currentTurn}/{turnLimit}");
        Console.WriteLine($"Current Cash: {currentCash}");
        Console.WriteLine($"Starting Cash: {startingCash}");
        Console.WriteLine($"Your Blue Beans: {blueBeans}");
        Console.WriteLine($"Your Red Beans: {redBeans}");
        Console.WriteLine($"Your Yellow Beans: {yellowBeans}");
        Console.WriteLine($"Your Green Beans: {greenBeans}");
        Console.WriteLine("------------------");
    }

    private bool validateBuyOrder(int amount, int price)
    {
        return 0 <= currentCash - (amount * price);
    }

    private bool validateSellOrder(int amount, int playerStock)
    {
        return playerStock - amount >= 0;
    }

    private void PlayerActions(Game gameData)
    {
        Console.WriteLine("Select one of the following");
        int index = 0;
        foreach (CityData city in cityList)
        {
            Console.WriteLine($"{index}: {city.cityName}");
            index++;
        }
        Console.WriteLine($"4: Next Turn");
        Console.WriteLine($"5: Exit Game (progress WILL NOT SAVE)");
        Console.WriteLine("entering a non-number or invalid number will restart the selection");
        var citySelection = Console.ReadLine();
        switch (citySelection)
        {
            case "0":
                TransactionSelection(cityList[0], gameData);
                break;
            case "1":
                TransactionSelection(cityList[1], gameData);
                break;
            case "2":
                TransactionSelection(cityList[2], gameData);
                break;
            case "3":
                TransactionSelection(cityList[3], gameData);
                break;
            case "4":
                break;
            case "5":
                exitGameFlag = true;
                TurnLoop(gameData);
                break;
            default:
                Console.WriteLine("Invalid selection! try again!");
                PlayerActions(gameData);
                break;
        }
    }

    private void TransactionSelection(CityData city, Game gameData)
    {
        Console.WriteLine($"Bean Market in {city.cityName}:");
        Console.WriteLine($"0: Blue Bean price: {city.blueBeanPrice}");
        Console.WriteLine($"1: Red Bean price: {city.redBeanPrice}");
        Console.WriteLine($"2: Yellow Bean price: {city.yellowBeanPrice}");
        Console.WriteLine($"3: Green Bean price: {city.greenBeanPrice}");
        Console.WriteLine($"4: Back to City Selection");
        Console.WriteLine("entering a non-number or invalid number will restart the selection");
        var beanSelection = Console.ReadLine();
        switch (beanSelection)
        {
            case "0":
                BuySellOrder(city, "Blue", gameData);
                break;
            case "1":
                BuySellOrder(city, "Red", gameData);
                break;
            case "2":
                BuySellOrder(city, "Yellow", gameData);
                break;
            case "3":
                BuySellOrder(city, "Green", gameData);
                break;
            case "4":
                PlayerActions(gameData);
                break;
            default:
                Console.WriteLine("Invalid selection! try again!");
                TransactionSelection(city, gameData);
                break;
        }
    }

    private void BuySellOrder(CityData city, string beanType, Game gameData)
    {
        Console.WriteLine($"Buy or Sell {beanType} Beans?");
        Console.WriteLine("0: Buy");
        Console.WriteLine("1: Sell");
        Console.WriteLine("2: Back to Bean Selection");
        var buySellSelection = Console.ReadLine();
        switch (buySellSelection)
        {
            case "0":
                Buying(city, beanType, gameData);
                break;
            case "1":
                Selling(city, beanType, gameData);
                break;
            case "2":
                TransactionSelection(city, gameData);
                break;
            default:
                Console.WriteLine("Invalid selection! try again!");
                BuySellOrder(city, beanType, gameData);
                break;
        }
    }

    private void Buying(CityData city, string beanType, Game gameData)
    {
        int beanPrice = 0;
        switch (beanType)
        {
            case "Blue":
                beanPrice = city.blueBeanPrice;
                break;
            case "Red":
                beanPrice = city.redBeanPrice;
                break;
            case "Yellow":
                beanPrice = city.yellowBeanPrice;
                break;
            case "Green":
                beanPrice = city.greenBeanPrice;
                break;
        }
        Console.WriteLine($"How many {beanType} Beans would you like to purchase? type 0 or a negative number to cancel");
        var amountInput = Console.ReadLine();
        int amount = 0;
        if (int.TryParse(amountInput, out int x))
        {
            amount = int.Parse(amountInput);
        }
        else
        {
            Console.WriteLine("Invalid amount! try again!");
            Buying(city, beanType, gameData);
        }

        if (amount <= 0)
        {
            BuySellOrder(city, beanType, gameData);
        }
        else
        {
            if (validateBuyOrder(amount, beanPrice))
            {
                Console.WriteLine($"{amount} {beanType} Beans bought at {beanPrice} each for a total of {amount * beanPrice}");
                currentCash = currentCash - (amount * beanPrice);
                switch (beanType)
                {
                    case "Blue":
                        blueBeans += amount;
                        break;
                    case "Red":
                        redBeans += amount;
                        break;
                    case "Yellow":
                        yellowBeans += amount;
                        break;
                    case "Green":
                        greenBeans += amount;
                        break;
                }
                Console.WriteLine($"{currentCash} remaining in balance");
            }
            else
            {
                Console.WriteLine("Invalid amount! try again!");
                Buying(city, beanType, gameData);
            }
        }
    }

    private void Selling(CityData city, string beanType, Game gameData)
    {
        int beanPrice = 0;
        int beanAmount = 0;
        switch (beanType)
        {
            case "Blue":
                beanPrice = city.blueBeanPrice;
                beanAmount = blueBeans;
                break;
            case "Red":
                beanPrice = city.redBeanPrice;
                beanAmount = redBeans;
                break;
            case "Yellow":
                beanPrice = city.yellowBeanPrice;
                beanAmount = yellowBeans;
                break;
            case "Green":
                beanPrice = city.greenBeanPrice;
                beanAmount = greenBeans;
                break;
        }
        Console.WriteLine($"How many {beanType} Beans would you like to sell? type 0 or a negative number to cancel");
        var amountInput = Console.ReadLine();
        int amount = 0;
        if (int.TryParse(amountInput, out int x))
        {
            amount = int.Parse(amountInput);
        }
        else
        {
            Console.WriteLine("Invalid amount! try again!");
            Selling(city, beanType, gameData);
        }

        if (amount <= 0)
        {
            BuySellOrder(city, beanType, gameData);
        }
        else
        {
            if (validateSellOrder(amount, beanAmount))
            {
                Console.WriteLine($"{amount} {beanType} Beans sold at {beanPrice} each for a total of {amount * beanPrice}");
                currentCash = currentCash + (amount * beanPrice);
                switch (beanType)
                {
                    case "Blue":
                        blueBeans -= amount;
                        Console.WriteLine($"{blueBeans} Blue Beans remaining in balance");
                        break;
                    case "Red":
                        redBeans -= amount;
                        Console.WriteLine($"{redBeans} Red Beans remaining in balance");
                        break;
                    case "Yellow":
                        yellowBeans -= amount;
                        Console.WriteLine($"{yellowBeans} Yellow Beans remaining in balance");
                        break;
                    case "Green":
                        greenBeans -= amount;
                        Console.WriteLine($"{greenBeans} Green Beans remaining in balance");
                        break;
            }
            }
            else
            {
                Console.WriteLine("Invalid amount! try again!");
                Selling(city, beanType, gameData);
            }
        }

    }

    private void GameEnd(Game gameData)
    {
        Console.WriteLine("--GAME ENDED--");
        Console.WriteLine($"Started the game with {startingCash} in cash");
        Console.WriteLine($"Ended the game with {currentCash} in cash after {currentTurn} turns");
        Console.WriteLine($"earned {currentCash - startingCash} in total profit over this period");
    }
}