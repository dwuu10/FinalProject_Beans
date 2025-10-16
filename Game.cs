using System;
using System.Transactions;

namespace Final_Project_Beans;

public class Game
{
    // initialize new game
    public Game(int startCash, int maxTurns, string[] cityNames, bool cheatMode)
    {
        //set cash and turns
        startingCash = startCash;
        currentCash = startingCash;
        turnLimit = maxTurns;
        currentTurn = 0;
        cheats = cheatMode;

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

    // load saved game constructor
    public Game(int startCash, int maxTurns, string[] cityNames, bool cheatMode, int cash, int turn, int blue, int red, int green, int yellow)
    {
        //set cash and turns
        startingCash = startCash;
        currentCash = cash;
        turnLimit = maxTurns;
        currentTurn = turn;
        cheats = cheatMode;

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
    CityData[] cityList;
    public int turnLimit;
    public int startingCash;
    public int currentTurn;
    public int currentCash;
    public int blueBeans;
    public int redBeans;
    public int yellowBeans;
    public int greenBeans;

    public bool cheats;

    // start new game
    public static void NewGame()
    {
        // default values
        var cityStrings = new string[] { "Seattle", "Stockholm", "Rome", "Manila" };
        int x = 0;
        int defaultCash = 1000;
        int defaultTurns = 10;
        // player enters amount of cash they want to start with, if invalid input, dont update from default
        Console.WriteLine("enter the amount of cash you would to start with, entering a non-number will default to 1000");
        var customCashInput = Console.ReadLine();
        var customCash = defaultCash;
        if (int.TryParse(customCashInput, out x))
        {
            customCash = int.Parse(customCashInput);
        }
        // ditto but with turn limit
        Console.WriteLine("enter the amount of turns you would like to play to, entering a non-number will default to 10");
        var customTurnsInput = Console.ReadLine();
        var customTurns = defaultTurns;
        if (int.TryParse(customTurnsInput, out x))
        {
            customTurns = int.Parse(customTurnsInput);
        }
        // if player enters 1, cheats are enabled, otherwise, no cheats
        Console.WriteLine("type 1 to enable cheats, leave empty or type anything else to play without cheats");
        var cheatsInput = Console.ReadLine();
        bool cheatsEnabled;
        if (cheatsInput == "1")
        {
            cheatsEnabled = true;
        }
        else
        {
            cheatsEnabled = false;
        }
        // creates new game object, which contains all important game data
        var gameObj = new Game(customCash, customTurns, cityStrings, cheatsEnabled);
        gameObj.TurnLoop(gameObj);
    }

    // actually start the game, options for user to create new or load a saved game
    public static void StartGame()
    {
        Console.WriteLine("New game or load game?");
        Console.WriteLine("0: New Game");
        Console.WriteLine("1: Load Game");
        Console.WriteLine("2: Exit Program");

        var input = Console.ReadLine();

        switch (input)
        {
            case "0":
                NewGame();
                break;
            case "1":
                LoadGame();
                break;
            case "2":
                // close the program
                System.Environment.Exit(0);
                break;
            default:
                // if invalid input, call the function again to try again
                Console.WriteLine("Invalid Input! try again");
                StartGame();
                break;
        }
    }

    // handles each game turn
    private void TurnLoop(Game gameData)
    {
        // end game if forced or turn limit is reached
        if (gameData.exitGameFlag == true || currentTurn > turnLimit)
        {
            GameEnd(gameData);
            return;
        }
        else
        {
            // update city data
            var currentCityData = TurnEvents.UpdateCityWeather(gameData.cityList);
            Console.WriteLine($"--BEGINNING TURN {gameData.currentTurn}--");
            Console.WriteLine("--CITY DATA UPDATED--");
            gameData.cityList = currentCityData;
            // handle turn events
            TurnEvents.TurnEventHandler(currentCityData);
            // print data and get player inputs
            gameData.PrintCityData();
            gameData.PrintPlayerData();
            gameData.PlayerActions(gameData);
            gameData.currentTurn++;
            // next turn
            TurnLoop(gameData);
        }
    }
    
    // prints important city and player data
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

    // transaction validation functions, used later
    private bool validateBuyOrder(int amount, int price)
    {
        return 0 <= currentCash - (amount * price);
    }

    private bool validateSellOrder(int amount, int playerStock)
    {
        return playerStock - amount >= 0;
    }

    // initial player options for each turn
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
        Console.WriteLine($"5: Back to Main Menu (progress WILL NOT SAVE)");
        Console.WriteLine("s: save game");
        Console.WriteLine("entering a non-number or invalid number will restart the selection");
        // only display cheat options if cheat mode is enabled
        if (gameData.cheats)
        {
            Console.WriteLine($"6: cheat menu");
        }

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
                StartGame();
                break;
            case "6":
            // only available if cheats are enabled
                if (gameData.cheats)
                {
                    CheatMode(gameData);
                }
                else
                {
                    Console.WriteLine("cheats not available! try again!");
                    PlayerActions(gameData);
                }
                break;
            case "s":
            // save game
                SaveGame(gameData);
                break;
            default:
            // if invalid option, call function again
                Console.WriteLine("Invalid selection! try again!");
                PlayerActions(gameData);
                break;
        }
    }

    // if city is selected by player, have player select which bean they wish to trade
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
            // return to player actions menu
                PlayerActions(gameData);
                break;
            default:
            // if invalid option, call function again
                Console.WriteLine("Invalid selection! try again!");
                TransactionSelection(city, gameData);
                break;
        }
    }

    // if player has selected bean type, have them choose whether to buy or sell
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
            // return to selecting bean type
                TransactionSelection(city, gameData);
                break;
            default:
                // if invalid option, call function again
                Console.WriteLine("Invalid selection! try again!");
                BuySellOrder(city, beanType, gameData);
                break;
        }
    }

    // if player is buying, have them enter how many beans they wish to buy
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
            // invalid input handler, call function again
            Console.WriteLine("Invalid amount! try again!");
            Buying(city, beanType, gameData);
        }

        if (amount <= 0)
        {
            // if input is 0 or less, return to selecting buy or sell order
            BuySellOrder(city, beanType, gameData);
        }
        else
        {
            // order validation
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
                // if invalid amount, return to buy options
                Console.WriteLine("Invalid amount! try again!");
                Buying(city, beanType, gameData);
            }
        }
    }

    // if player is selling, have them enter how many they wish to sell
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
            // invalid input handler, call function again
            Console.WriteLine("Invalid amount! try again!");
            Selling(city, beanType, gameData);
        }

        if (amount <= 0)
        {
            // if input is 0 or less, return to selecting buy or sell order
            BuySellOrder(city, beanType, gameData);
        }
        else
        {
            // order validation
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
                // if invalid amount, return to sell options
                Console.WriteLine("Invalid amount! try again!");
                Selling(city, beanType, gameData);
            }
        }

    }

    // if game forced to end or reached the final turn, print out ending stats
    private void GameEnd(Game gameData)
    {
        Console.WriteLine("--GAME ENDED--");
        Console.WriteLine($"Started the game with {startingCash} in cash");
        Console.WriteLine($"Ended the game with {currentCash} in cash after {currentTurn} turns");
        Console.WriteLine($"earned {currentCash - startingCash} in total profit over this period");
        // return to starting game menu
        Console.WriteLine("ENTER TO RETURN TO MENU");
        Console.ReadLine();
        StartGame();
    }

    // if cheat mode is enabled and selected in player actions, will show cheat menu
    private void CheatMode(Game gameData)
    {
        Console.WriteLine($"0: Edit your cash");
        Console.WriteLine($"1: Edit your turn");
        Console.WriteLine($"2: Edit your beans");
        Console.WriteLine($"3: Go back");
        var cheatSelection = Console.ReadLine();
        switch (cheatSelection)
        {
            case "0":
                EditCash(gameData);
                break;
            case "1":
                EditTurns(gameData);
                break;
            case "2":
                EditBeans(gameData);
                break;
            case "3":
                PlayerActions(gameData);
                break;
            default:
                // invalid input handler
                Console.WriteLine("Invalid Input, please try again");
                CheatMode(gameData);
                break;
        }
    }

    // player wishes to edit their bean amounts with cheats
    private void EditBeans(Game gameData)
    {
        Console.WriteLine("0: Edit Blue Beans");
        Console.WriteLine("1: Edit Red Beans");
        Console.WriteLine("2: Edit Yellow Beans");
        Console.WriteLine("3: Edit Green Beans");
        Console.WriteLine("4: Go back");
        string beanType = "";
        var editBeanInput = Console.ReadLine();
        switch (editBeanInput)
        {
            case "0":
                beanType = "blue";
                BeanSetter(gameData, beanType);
                break;
            case "1":
                beanType = "red";
                BeanSetter(gameData, beanType);
                break;
            case "2":
                beanType = "yellow";
                BeanSetter(gameData, beanType);
                break;
            case "3":
                beanType = "green";
                BeanSetter(gameData, beanType);
                break;
            case "4":
            // return to cheat menu
                CheatMode(gameData);
                break;
            default:
            // invalid input handler
                Console.WriteLine("Invalid input, please try again");
                EditBeans(gameData);
                break;
        }
    }

    // player wishes to edit their cash with cheats
    private void EditCash(Game gameData)
    {
        Console.WriteLine("Set your cash");
        Console.WriteLine("b: Go back");
        var editCashInput = Console.ReadLine();
        if (editCashInput == "b")
        {
            // return to cheats menu
            CheatMode(gameData);
        }
        else
        {

            if (int.TryParse(editCashInput, out int x))
            {
                gameData.currentCash = int.Parse(editCashInput);
                PrintPlayerData();
                PlayerActions(gameData);
            }
            else
            {
                // invalid input handler
                Console.WriteLine("Invalid Input, please try again");
                EditCash(gameData);
            }
        }
    }

    // player wishes to edit their current turn with cheats
    private void EditTurns(Game gameData)
    {
        Console.WriteLine("Set your turn");
        Console.WriteLine("b: go back");
        var editTurnInput = Console.ReadLine();
        if (editTurnInput == "b")
        {
            // return to cheats menu
            CheatMode(gameData);
        }
        else
        {
            if (int.TryParse(editTurnInput, out int x))
            {
                gameData.currentTurn = int.Parse(editTurnInput);
                PrintPlayerData();
                PlayerActions(gameData);
            }
            else
            {
                // invalid input handler
                Console.WriteLine("Invalid Input, please try again");
                EditTurns(gameData);
            }
        }
    }

    // manually set number of selected bean type
    private void BeanSetter(Game gameData, string beanType)
    {
        Console.WriteLine($"Editing bean type: {beanType}");
        Console.WriteLine($"Set {beanType} amount");
        Console.WriteLine("b: Go back");
        var editCountInput = Console.ReadLine();
        int amountToChange;
        if (editCountInput == "b")
        {
            // return to edit beans selection
            EditBeans(gameData);
        }
        else
        {
            if (int.TryParse(editCountInput, out int x))
            {
                amountToChange = int.Parse(editCountInput);
                switch (beanType)
                {
                    case "blue":
                        gameData.blueBeans = amountToChange;
                        break;
                    case "red":
                        gameData.redBeans = amountToChange;
                        break;
                    case "yellow":
                        gameData.yellowBeans = amountToChange;
                        break;
                    case "green":
                        gameData.greenBeans = amountToChange;
                        break;
                }
                // return to player actions for that turn
                PrintPlayerData();
                PlayerActions(gameData);
            }
            else
            {
                // invalid input handler
                Console.WriteLine("Invalid Input, please try again");
                BeanSetter(gameData, beanType);
            }
        }
        CheatMode(gameData);
    }

    // load game from a .txt file in the "Saves" folder
    private static void LoadGame()
    {
        // Saves folder directory
        string mainDir = System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.AppContext.BaseDirectory).ToString()).ToString()).ToString()).ToString();
        var cityStrings = new string[] { "Seattle", "Stockholm", "Rome", "Manila" };

        // get player input for save file name
        Console.WriteLine("Type the name of the save file (no extensions)");
        Console.WriteLine("Enter a blank name to go back");
        string fileName = Console.ReadLine();
        if (fileName == "")
        {
            // return to start game menu if empty input
            StartGame();
        }

        try
        {
            // read selected file, each value is currently saved on a seperate line
            StreamReader reader = new StreamReader($"{mainDir}/Saves/{fileName}.txt");
            var startingCash = int.Parse(reader.ReadLine());
            var maxTurn = int.Parse(reader.ReadLine());
            var cheatsFile = reader.ReadLine();
            bool cheats = false;
            if (cheatsFile == "1")
            {
                cheats = true;
            }
            else
            {
                cheats = false;
            }
            var cash = int.Parse(reader.ReadLine());
            var turn = int.Parse(reader.ReadLine());
            var blue = int.Parse(reader.ReadLine());
            var red = int.Parse(reader.ReadLine());
            var green = int.Parse(reader.ReadLine());
            var yellow = int.Parse(reader.ReadLine());
            reader.Close();

            // create new game object with loaded data and begin turn loop
            var gameObj = new Game(startingCash, maxTurn, cityStrings, cheats, cash, turn, blue, red, green, yellow);
            gameObj.TurnLoop(gameObj);
        }
        catch (Exception)
        {
            // if something went wrong, return to start game menu after informing player
            Console.WriteLine("something went wrong, did you enter the name correctly?");
            StartGame();
        }
    }

    // saves game to a .txt file in the "Saves" folder
    private static void SaveGame(Game gameData)
    {
        // Saves folder directory
        string mainDir = System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.AppContext.BaseDirectory).ToString()).ToString()).ToString()).ToString();
        Console.WriteLine("Enter the file name (no extensions)");
        Console.WriteLine("Enter a blank name to go back");
        string fileName = Console.ReadLine();
        if (fileName == "")
        {
            // return to start menu if empty input
            StartGame();
        }

        // create new save file, cash, turn, and bean inventory data from currently running game written on seperate lines
        StreamWriter writer = new StreamWriter($"{mainDir}/Saves/{fileName}.txt");
        writer.WriteLine(gameData.startingCash);
        writer.WriteLine(gameData.turnLimit);
        if (gameData.cheats)
        {
            writer.WriteLine("1");
        }
        else
        {
            writer.WriteLine("0");
        }
        writer.WriteLine(gameData.currentCash);
        writer.WriteLine(gameData.currentTurn);
        writer.WriteLine(gameData.blueBeans);
        writer.WriteLine(gameData.redBeans);
        writer.WriteLine(gameData.greenBeans);
        writer.WriteLine(gameData.yellowBeans);
        writer.Close();
        // return to the game once saving is complete
        Console.WriteLine($"Game saved to Saves/{fileName}.txt");
        gameData.PlayerActions(gameData);
    }
}