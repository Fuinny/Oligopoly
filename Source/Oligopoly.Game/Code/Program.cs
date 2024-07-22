namespace Oligopoly.Game;

public class Program
{
    private readonly static List<Company> s_companies = [];
    private readonly static List<Event> s_events = [];
    private readonly static List<Event> s_globalEvents = [];

    private static bool s_isGameEnded = false;
    private static bool s_closeRequest = false;

    private static int s_gameMode;
    private static int s_gameDifficulty;
    private static int s_turnCounter;
    private static int s_skipTurnLimit;

    private static int s_positiveEventChance;

    private static decimal s_netWorth;
    private static decimal s_playerMoney;

    private const decimal LosingNetWorth = 2000.00M;
    private const decimal WinningNetWorth = 50000.00M;


    /// <summary>Contains the entry point and main logic of the game.</summary>
    private static void Main()
    {
        SetupConsoleEnvironment();

        while (!s_closeRequest)
        {
            switch (DisplayMainMenu())
            {
                case 0:
                    LoadEmbeddedResources();
                    DisplayGameSetupMenu(false);
                    DisplayIntroductionLetter();
                    GameLoop();
                    break;
                case 1:
                    if (LoadGame())
                    {
                        LoadEmbeddedResources();
                        DisplayGameSetupMenu(true);
                        GameLoop();
                    }
                    break;
                case 2:
                    DisplayAboutGameMenu();
                    break;
                case 3:
                    DisplayExitMenu();
                    break;
            }
        }
    }

    #region GameSetupAndDataHandling
    /// <summary>Sets up the environment for the game, including Saves folder creation and console settings (Windows only).</summary>
    private static void SetupConsoleEnvironment()
    {
        Console.OutputEncoding = Encoding.UTF8;
        if (!Directory.Exists("Saves")) Directory.CreateDirectory("Saves");
        if (OperatingSystem.IsWindows())
        {
            Console.CursorVisible = false;
            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight;
        }
    }

    /// <summary>Loads .xml files from Data folder into their respective collections.</summary>
    private static void LoadEmbeddedResources()
    {
        try
        {
            XDocument xmlDocument;

            xmlDocument = XDocument.Load(Path.Combine("Data", "Companies.xml"));
            foreach (XElement companyElement in xmlDocument.Root.Elements("Company"))
            {
                Company currentCompany = new()
                {
                    Name = companyElement.Element("Name").Value,
                    Industry = companyElement.Element("Industry").Value,
                    SharePrice = decimal.Parse(companyElement.Element("SharePrice").Value),
                    Description = companyElement.Element("Description").Value
                };

                s_companies.Add(currentCompany);
            }

            xmlDocument = XDocument.Load(Path.Combine("Data", "Events.xml"));
            foreach (XElement eventElement in xmlDocument.Root.Elements("Event"))
            {
                Event currentEvent = new()
                {
                    Effect = int.Parse(eventElement.Element("Effect").Value),
                    Target = eventElement.Element("Target").Value,
                    Title = eventElement.Element("Title").Value,
                    Content = eventElement.Element("Content").Value
                };

                s_events.Add(currentEvent);
            }

            xmlDocument = XDocument.Load(Path.Combine("Data", "GlobalEvents.xml"));
            foreach (XElement globalEventElement in xmlDocument.Root.Elements("GlobalEvent"))
            {
                Event currentGlobalEvent = new()
                {
                    Effect = int.Parse(globalEventElement.Element("Effect").Value),
                    Target = "All",
                    Title = globalEventElement.Element("Title").Value,
                    Content = globalEventElement.Element("Content").Value
                };

                s_globalEvents.Add(currentGlobalEvent);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error! \nDetails: {ex.Message}");
            Console.WriteLine("Press any key to exit the game...");
            Console.ReadKey(true);
            Environment.Exit(0);
        }
    }

    /// <summary>Saves the current state of the game to .xml file in Saves folder.</summary>
    private static void SaveGame()
    {
        int saveFileCounter = 1;
        string saveFileName = $"Save_{saveFileCounter}.xml";
        string saveFilePath = Path.Combine("Saves", saveFileName);

        try
        {
            while (File.Exists(saveFilePath))
            {
                saveFileCounter++;
                saveFileName = $"Save_{saveFileCounter}.xml";
                saveFilePath = Path.Combine("Saves", saveFileName);
            }

            XDocument saveFile = new(
                new XElement("SaveFile",
                    new XElement("GameMode", s_gameMode),
                    new XElement("Difficulty", s_gameDifficulty),
                    new XElement("CurrentTurn", s_turnCounter),
                    new XElement("PlayerMoney", s_playerMoney.ToString(CultureInfo.CurrentCulture)),
                    new XElement("SharePrices", s_companies.Select(company => new XElement($"{company.Name.Replace(" ", "_")}", company.SharePrice.ToString(CultureInfo.CurrentCulture)))),
                    new XElement("BuyedShares", s_companies.Select(company => new XElement($"{company.Name.Replace(" ", "_")}", company.NumberOfShares)))
                )
            );

            saveFile.Save(saveFilePath);

            Console.WriteLine($"\nYour file was successfully saved with the name {saveFileName}");
            Console.WriteLine("You can find all of your save files in Saves folder.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error! \nDetails: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("\nPress any key to exit the menu...");
            Console.ReadKey(true);
        }
    }

    /// <summary>Loads the game from already created .xml files from Saves folder.</summary>
    private static bool LoadGame()
    {
        string[] saveFilesPaths = Directory.GetFiles("Saves", "*.xml");
        string[] saveFileNames = Array.ConvertAll(saveFilesPaths, Path.GetFileName);

        if (saveFilesPaths.Length == 0)
        {
            Console.Clear();
            Console.WriteLine("No save files were found.");
            Console.WriteLine("\nPress any key to exit the menu...");
            Console.ReadKey(true);
            return false;
        }

        Menu loadMenu = new("Select file to load", saveFileNames);
        int selectedSaveFile = loadMenu.RunMenu();

        try
        {
            XDocument saveFile = XDocument.Load(saveFilesPaths[selectedSaveFile]);
            s_gameMode = int.Parse(saveFile.Element("SaveFile").Element("GameMode").Value);
            s_gameDifficulty = int.Parse(saveFile.Element("SaveFile").Element("Difficulty").Value);
            s_turnCounter = int.Parse(saveFile.Element("SaveFile").Element("CurrentTurn").Value);
            s_playerMoney = decimal.Parse(saveFile.Element("SaveFile").Element("PlayerMoney").Value);
            var companySharePrices = saveFile.Element("SaveFile").Element("SharePrices").Elements();
            var playerBuyedShares = saveFile.Element("SaveFile").Element("BuyedShares").Elements();

            foreach (var companyElement in companySharePrices)
            {
                string companyName = companyElement.Name.LocalName.Replace("_", " ");
                decimal sharePrice = decimal.Parse(companyElement.Value);

                Company company = s_companies.FirstOrDefault(c => c.Name == companyName);
                if (company != null)
                {
                    company.SharePrice = sharePrice;
                }
            }

            foreach (var companyElement in playerBuyedShares)
            {
                string companyName = companyElement.Name.LocalName.Replace('_', ' ');
                int numberOfShares = int.Parse(companyElement.Value);

                Company company = s_companies.FirstOrDefault(c => c.Name == companyName);
                if (company != null)
                {
                    company.NumberOfShares = numberOfShares;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error! \nDetails: {ex.Message}");
            Console.WriteLine("Press any key to exit the menu...");
            Console.ReadKey(true);
            return false;
        }

        return true;
    }
    #endregion

    #region GameMenus
    /// <summary>Displays main menu to the console.</summary>
    /// <returns>An <see cref="System.Int32"/> value, that represents selected option's index.</returns>
    private static int DisplayMainMenu()
    {
        string prompt = @" 
 ██████╗ ██╗     ██╗ ██████╗  ██████╗ ██████╗  ██████╗ ██╗  ██╗   ██╗
██╔═══██╗██║     ██║██╔════╝ ██╔═══██╗██╔══██╗██╔═══██╗██║  ╚██╗ ██╔╝
██║   ██║██║     ██║██║  ███╗██║   ██║██████╔╝██║   ██║██║   ╚████╔╝ 
██║   ██║██║     ██║██║   ██║██║   ██║██╔═══╝ ██║   ██║██║    ╚██╔╝  
╚██████╔╝███████╗██║╚██████╔╝╚██████╔╝██║     ╚██████╔╝███████╗██║   
 ╚═════╝ ╚══════╝╚═╝ ╚═════╝  ╚═════╝ ╚═╝      ╚═════╝ ╚══════╝╚═╝   
           Use up and down arrow keys to select an option             
";
        string[] options = ["Play", "Load", "About", "Exit"];
        Menu mainMenu = new(prompt, options);
        return mainMenu.RunMenu();
    }

    /// <summary>Displays pause menu to the console.</summary>
    private static void DisplayPauseMenu()
    {
        string prompt = @"
 ██████╗  █████╗ ███╗   ███╗███████╗     ██████╗ ███╗   ██╗    ██████╗  █████╗ ██╗   ██╗███████╗███████╗
██╔════╝ ██╔══██╗████╗ ████║██╔════╝    ██╔═══██╗████╗  ██║    ██╔══██╗██╔══██╗██║   ██║██╔════╝██╔════╝
██║  ███╗███████║██╔████╔██║█████╗      ██║   ██║██╔██╗ ██║    ██████╔╝███████║██║   ██║███████╗█████╗  
██║   ██║██╔══██║██║╚██╔╝██║██╔══╝      ██║   ██║██║╚██╗██║    ██╔═══╝ ██╔══██║██║   ██║╚════██║██╔══╝  
╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗    ╚██████╔╝██║ ╚████║    ██║     ██║  ██║╚██████╔╝███████║███████╗
 ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝     ╚═════╝ ╚═╝  ╚═══╝    ╚═╝     ╚═╝  ╚═╝ ╚═════╝ ╚══════╝╚══════╝
                           Use up and down arrow keys to select an option                                
";
        string[] options = ["Save", "Load", "Exit"];
        Menu pauseMenu = new(prompt, options, true);
        switch (pauseMenu.RunMenu())
        {
            case 0:
                SaveGame();
                break;
            case 1:
                LoadGame();
                break;
            case 2:
                DisplayExitMenu();
                break;
        }
    }

    /// <summary>Displays game setup menu to the console.</summary>
    /// <param name="isLoading">Indicates whether the player is loading a saved game or starting a new game.</param>
    private static void DisplayGameSetupMenu(bool isLoading)
    {
        string prompt = @"
 ██████╗  █████╗ ███╗   ███╗███████╗    ███████╗███████╗████████╗██╗   ██╗██████╗ 
██╔════╝ ██╔══██╗████╗ ████║██╔════╝    ██╔════╝██╔════╝╚══██╔══╝██║   ██║██╔══██╗
██║  ███╗███████║██╔████╔██║█████╗      ███████╗█████╗     ██║   ██║   ██║██████╔╝
██║   ██║██╔══██║██║╚██╔╝██║██╔══╝      ╚════██║██╔══╝     ██║   ██║   ██║██╔═══╝ 
╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗    ███████║███████╗   ██║   ╚██████╔╝██║     
 ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝    ╚══════╝╚══════╝   ╚═╝    ╚═════╝ ╚═╝     
         Customize the game to make it interesting for you to play ;)              
";
        if (!isLoading)
        {
            string[] difficultiesOptions = ["Easy", "Normal", "Hard"];
            string[] difficultiesDescriptions = 
            [
                "The limit of turns you can skip will be set to 10 \n60% chance that the next market event will be POSITIVE",
                "The limit of turns you can skip will be set to 5 \n50% chance that the next market event will be POSITIVE/NEGATIVE",
                "The limit of turns you can skip will be set to 3 \n60% chance that the next market event will be NEGATIVE"
            ];
            Menu difficultiesMenu = new(prompt, difficultiesOptions, false, difficultiesDescriptions);

            string[] gameModesOptions = ["Standard", "Random", "Custom"];
            string[] gameModesDescriptions =
            [
                "You start the game with default values",
                "Your starting amount of money and company share prices will be randomly generated",
                "You can set the starting amount of money by yourself"
            ];
            Menu gameModesMenu = new(prompt, gameModesOptions, false, gameModesDescriptions);

            s_turnCounter = 1;
            s_playerMoney = 10000.00M;
            s_gameDifficulty = difficultiesMenu.RunMenu();
            s_gameMode = gameModesMenu.RunMenu();

            switch(s_gameMode)
            {
                case 1:
                    s_playerMoney = Random.Shared.Next(1000, 30001);
                    foreach (Company company in s_companies)
                    {
                        company.SharePrice = Random.Shared.Next(100, 5001);
                    }
                    break;
                case 2:
                    DisplayMoneySetupMenu();
                    break;
            }    
        }

        switch (s_gameDifficulty)
        {
            case 0:
                s_skipTurnLimit = 10;
                s_positiveEventChance = 60; 
                break;
            case 1:
                s_skipTurnLimit = 5;
                s_positiveEventChance = 50;
                break;
            case 2:
                s_skipTurnLimit = 3;
                s_positiveEventChance = 40;
                break;
        }
    }

    /// <summary>Displays money setup menu to the console.</summary>
    private static void DisplayMoneySetupMenu()
    {
        string prompt = @"
███╗   ███╗ ██████╗ ███╗   ██╗███████╗██╗   ██╗    ███████╗███████╗████████╗██╗   ██╗██████╗ 
████╗ ████║██╔═══██╗████╗  ██║██╔════╝╚██╗ ██╔╝    ██╔════╝██╔════╝╚══██╔══╝██║   ██║██╔══██╗
██╔████╔██║██║   ██║██╔██╗ ██║█████╗   ╚████╔╝     ███████╗█████╗     ██║   ██║   ██║██████╔╝
██║╚██╔╝██║██║   ██║██║╚██╗██║██╔══╝    ╚██╔╝      ╚════██║██╔══╝     ██║   ██║   ██║██╔═══╝ 
██║ ╚═╝ ██║╚██████╔╝██║ ╚████║███████╗   ██║       ███████║███████╗   ██║   ╚██████╔╝██║     
╚═╝     ╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚══════╝   ╚═╝       ╚══════╝╚══════╝   ╚═╝    ╚═════╝ ╚═╝     
                        Customize your starting amount of money                               
";
        string[] options = ["(+) Increase", "(-) Decrease", "Done"];
        Menu moneySetupMenu = new(prompt, options);
        s_playerMoney = moneySetupMenu.RunMoneySetupMenu();
    }

    /// <summary>Displays companies description to the console.</summary>
    private static void DisplayMoreAboutCompaniesMenu()
    {
        string header = @"
 █████╗ ██████╗  ██████╗ ██╗   ██╗████████╗     ██████╗ ██████╗ ███╗   ███╗██████╗  █████╗ ███╗   ██╗██╗███████╗███████╗
██╔══██╗██╔══██╗██╔═══██╗██║   ██║╚══██╔══╝    ██╔════╝██╔═══██╗████╗ ████║██╔══██╗██╔══██╗████╗  ██║██║██╔════╝██╔════╝
███████║██████╔╝██║   ██║██║   ██║   ██║       ██║     ██║   ██║██╔████╔██║██████╔╝███████║██╔██╗ ██║██║█████╗  ███████╗
██╔══██║██╔══██╗██║   ██║██║   ██║   ██║       ██║     ██║   ██║██║╚██╔╝██║██╔═══╝ ██╔══██║██║╚██╗██║██║██╔══╝  ╚════██║
██║  ██║██████╔╝╚██████╔╝╚██████╔╝   ██║       ╚██████╗╚██████╔╝██║ ╚═╝ ██║██║     ██║  ██║██║ ╚████║██║███████╗███████║
╚═╝  ╚═╝╚═════╝  ╚═════╝  ╚═════╝    ╚═╝        ╚═════╝ ╚═════╝ ╚═╝     ╚═╝╚═╝     ╚═╝  ╚═╝╚═╝  ╚═══╝╚═╝╚══════╝╚══════╝
                                Select the company you want to learn more about
";
        string[] companyNames = new string[s_companies.Count + 1];
        for (int i = 0; i < s_companies.Count; i++)
        {
            companyNames[i] = s_companies[i].Name;
        }
        companyNames[s_companies.Count] = "Back";

        bool exitSelected = false;
        while (!exitSelected)
        {
            Menu companySelectionMenu = new(header, companyNames);
            int selectedCompany = companySelectionMenu.RunMenu();

            if (selectedCompany == s_companies.Count)
            {
                exitSelected = true;
            }
            else
            {
                StringBuilder companyDetails = new();
                companyDetails.AppendLine(header);
                companyDetails.AppendLine($"{s_companies[selectedCompany].Name}:");
                companyDetails.AppendLine(s_companies[selectedCompany].Description);
                string[] options = ["Continue"];
                Menu companyDetailsMenu = new(companyDetails.ToString(), options);
                companyDetailsMenu.RunMenu();
            }
        }
    }

    /// <summary>Displays thanks to the player and information about the creator of the game.</summary>
    private static void DisplayAboutGameMenu()
    {
        string prompt = @"
          ████████╗██╗  ██╗ █████╗ ███╗   ██╗██╗  ██╗███████╗██╗                    
          ╚══██╔══╝██║  ██║██╔══██╗████╗  ██║██║ ██╔╝██╔════╝██║                    
             ██║   ███████║███████║██╔██╗ ██║█████╔╝ ███████╗██║                    
             ██║   ██╔══██║██╔══██║██║╚██╗██║██╔═██╗ ╚════██║╚═╝                    
             ██║   ██║  ██║██║  ██║██║ ╚████║██║  ██╗███████║██╗                    
             ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝╚═╝  ╚═╝╚══════╝╚═╝                    
╔══════════════════════════════════════════════════════════════════════════════════╗
║No really, thank you for taking time to play this simple CLI game. It means a lot.║
║If you find any bug or have an idea how to improve the game, please let me know :D║
║                                                                                  ║
║This game was created by Semion Medvedev (Fuinny)                                 ║
╚══════════════════════════════════════════════════════════════════════════════════╝ 
";
        string[] options = ["Return to Main Menu"];
        Menu aboutGameMenu = new(prompt, options);
        aboutGameMenu.RunMenu();
    }

    /// <summary>Displays exit menu to the console.</summary>
    private static void DisplayExitMenu()
    {
        string prompt = @"Are you sure you want to exit the game?";
        string[] options = ["Exit the Game", "Back"];
        Menu exitMenu = new(prompt, options);
        switch (exitMenu.RunMenu())
        {
            case 0:
                s_isGameEnded = true;
                s_closeRequest = true;
                break;
        }
    }
    #endregion

    #region GameCore
    /// <summary>Calculates current net worth.</summary>
    private static void CalculateNetWorth()
    {
        s_netWorth = s_playerMoney;
        foreach (Company company in s_companies)
        {
            s_netWorth += company.NumberOfShares * company.SharePrice;
        }
    }
    
    /// <summary>Changes the share price of all companies from -3 to 3 per cent.</summary>
    private static void UpdateMarketPrices()
    {
        for (int i = 0; i < s_companies.Count; i++)
        {
            Random random = new();
            int effect = random.Next(-3, 4);

            s_companies[i].SharePrice += s_companies[i].SharePrice * effect / 100;
        }
    }

    /// <summary>Generates random event or random global event.</summary>
    private static void GenerateRandomEvent()
    {
        Event currentEvent;
        StringBuilder prompt = Menu.DrawCompaniesTable(s_companies);

        bool isPositive = Random.Shared.Next(0, 101) <= s_positiveEventChance;

        if (s_turnCounter % 50 == 0)
        {
            if (isPositive)
            {
                List<Event> positiveGlobalEvents = s_globalEvents.Where(e => e.Effect > 0).ToList();
                currentEvent = positiveGlobalEvents[Random.Shared.Next(0, positiveGlobalEvents.Count)];
            }
            else
            {
                List<Event> negativeGlobalEvents = s_globalEvents.Where(e => e.Effect < 0).ToList();
                currentEvent = negativeGlobalEvents[Random.Shared.Next(0, negativeGlobalEvents.Count)];
            }
        }
        else
        {
            if (isPositive)
            {
                List<Event> positiveEvents = s_events.Where(e => e.Effect > 0).ToList();
                currentEvent = positiveEvents[Random.Shared.Next(0, positiveEvents.Count)];
            }
            else
            {
                List<Event> negativeEvents = s_events.Where(e => e.Effect < 0).ToList();
                currentEvent = negativeEvents[Random.Shared.Next(0, negativeEvents.Count)];
            }
        }

        prompt.AppendLine($"\n{currentEvent.Content}");
        string[] options = ["Continue"];
        Menu eventMenu = new(prompt.ToString(), options);
        eventMenu.RunMenu();
    }

    /// <summary>Displays buy or sell menu to the console.</summary>
    /// <param name="isBuying">Determines whether player is buying or selling shares.</param>
    private static void DisplayBuyOrSellMenu(bool isBuying)
    {
        StringBuilder prompt = Menu.DrawCompaniesTable(s_companies);
        prompt.AppendLine($"\nYou have: {Math.Round(s_playerMoney, 2):C}");
        prompt.AppendLine($"\nUse the arrow keys to select and enter to confirm how many shares to {(isBuying ? "buy" : "sell")}");

        decimal transactionValue = 0.0M;
        int[] numberOfSharesToProcess = new int[s_companies.Count];
        string[] options = new string[s_companies.Count];

        for (int i = 0; i < s_companies.Count; i++)
        {
            options[i] = s_companies[i].Name;
        }

        Menu buyOrSellMenu = new(prompt.ToString(), options);
        buyOrSellMenu.RunTransactionMenu(ref numberOfSharesToProcess, ref transactionValue, s_playerMoney, s_companies, isBuying);

        for (int i = 0; i < numberOfSharesToProcess.Length; i++)
        {
            if (isBuying)
            {
                s_playerMoney -= numberOfSharesToProcess[i] * s_companies[i].SharePrice;
                s_companies[i].NumberOfShares += numberOfSharesToProcess[i];
            }
            else
            {
                s_playerMoney += numberOfSharesToProcess[i] * s_companies[i].SharePrice;
                s_companies[i].NumberOfShares -= numberOfSharesToProcess[i];
            }
        }
    }

    /// <summary>Runs a game session.</summary>
    private static void GameLoop()
    {
        s_isGameEnded = false;
        int skipCount = 0;

        while (!s_isGameEnded)
        {
            CalculateNetWorth();

            StringBuilder prompt = Menu.DrawCompaniesTable(s_companies);
            prompt.AppendLine($"\nYou have: {Math.Round(s_playerMoney, 2):C}    Your Net Worth: {Math.Round(s_netWorth, 2):C}    Current Turn: {s_turnCounter}");
            string[] options = ["Wait for Market Change", "Buy", "Sell", "More About Companies"];

            Menu gameMenu = new(prompt.ToString(), options, true);

            switch (gameMenu.RunMenu())
            {
                case -1:
                    DisplayPauseMenu();
                    continue;
                case 0:
                    if (skipCount == s_skipTurnLimit)
                    {
                        Console.WriteLine($"\nYou cannot skip more than {s_skipTurnLimit} turns!");
                        Console.ReadKey(true);
                        continue;
                    }
                    else
                    {
                        UpdateMarketPrices();
                        GenerateRandomEvent();
                        skipCount++;
                    }
                    break;
                case 1:
                    DisplayBuyOrSellMenu(true);
                    skipCount = 0;
                    break;
                case 2:
                    DisplayBuyOrSellMenu(false);
                    skipCount = 0;
                    break;
                case 3:
                    DisplayMoreAboutCompaniesMenu();
                    continue;
            }

            switch (s_netWorth)
            {
                case < LosingNetWorth:
                    s_isGameEnded = true;
                    DisplayLoseLetter();
                    break;
                case > WinningNetWorth:
                    s_isGameEnded = true;
                    DisplayWinLetter();
                    break;
            }

            s_turnCounter++;
        }

        s_companies.Clear();
        s_events.Clear();
        s_globalEvents.Clear();
    }
    #endregion

    #region Letters
    /// <summary>Displays introduction letter to the console.</summary>
    private static void DisplayIntroductionLetter()
    {
        string prompt = @"
          ██╗    ██╗███████╗██╗      ██████╗ ██████╗ ███╗   ███╗███████╗          
          ██║    ██║██╔════╝██║     ██╔════╝██╔═══██╗████╗ ████║██╔════╝          
          ██║ █╗ ██║█████╗  ██║     ██║     ██║   ██║██╔████╔██║█████╗            
          ██║███╗██║██╔══╝  ██║     ██║     ██║   ██║██║╚██╔╝██║██╔══╝            
          ╚███╔███╔╝███████╗███████╗╚██████╗╚██████╔╝██║ ╚═╝ ██║███████╗          
           ╚══╝╚══╝ ╚══════╝╚══════╝ ╚═════╝ ╚═════╝ ╚═╝     ╚═╝╚══════╝          
╔════════════════════════════════════════════════════════════════════════════════╗
║ Dear new CEO,                                                                  ║
║                                                                                ║
║ Welcome to Oligopoly!                                                          ║
║                                                                                ║
║ On behalf of the board of directors of Oligopoly Investments, we would like to ║
║ congratulate you on becoming our new CEO. We are confident that you will lead  ║
║ our company to new heights of success and innovation. As CEO, you now have     ║
║ access to our exclusive internal software called Oligopoly, where you can      ║
║ track the latest news from leading companies and buy and sell their shares.    ║
║ This software will give you an edge over the competition and help you make     ║
║ important decisions for our company. To access the program, simply click the   ║
║ button at the bottom of this email. We look forward to working with you and    ║
║ supporting you in your new role.                                               ║
║                                                                                ║
║ Sincerely,                                                                     ║
║ The board of directors of Oligopoly Investments                                ║
╚════════════════════════════════════════════════════════════════════════════════╝
";
        string[] options = ["Start the Game"];
        Menu introductionMenu = new(prompt, options);
        introductionMenu.RunMenu();
    }

    /// <summary>Displays win letter to the console.</summary>
    private static void DisplayWinLetter()
    {
        string prompt = $@"
          ██╗   ██╗ ██████╗ ██╗   ██╗    ██╗    ██╗██╗███╗   ██╗                  
          ╚██╗ ██╔╝██╔═══██╗██║   ██║    ██║    ██║██║████╗  ██║                  
           ╚████╔╝ ██║   ██║██║   ██║    ██║ █╗ ██║██║██╔██╗ ██║                  
            ╚██╔╝  ██║   ██║██║   ██║    ██║███╗██║██║██║╚██╗██║                  
             ██║   ╚██████╔╝╚██████╔╝    ╚███╔███╔╝██║██║ ╚████║                  
             ╚═╝    ╚═════╝  ╚═════╝      ╚══╝╚══╝ ╚═╝╚═╝  ╚═══╝                  
╔════════════════════════════════════════════════════════════════════════════════╗
║ Dear CEO,                                                                      ║
║                                                                                ║
║ On behalf of the board of directors of Oligopoly Investments, we would like to ║
║ express our gratitude and understanding for your decision to leave your post.  ║
║ You have been a remarkable leader and a visionary strategist, who played the   ║
║ stock market skillfully and increased our budget by five times. We are proud   ║
║ of your achievements and we wish you all the best in your future endeavors. As ║
║ a token of our appreciation, we are pleased to inform you that the company     ║
║ will pay you a bonus of $1 million. You deserve this reward for your hard work ║
║ and dedication. We hope you will enjoy it and remember us fondly. Thank you    ║
║ for your service and your contribution to Oligopoly Investments.               ║
║ You will be missed.                                                            ║
║                                                                                ║
║ Sincerely,                                                                     ║
║ The board of directors of Oligopoly Investments                                ║
╚════════════════════════════════════════════════════════════════════════════════╝
                                                                                  
Your Net Worth is over {WinningNetWorth}$
You have played {s_turnCounter} turns
Congratulations!";
        string[] options = ["Continue"];
        Menu winMenu = new(prompt, options);
        winMenu.RunMenu();
    }

    /// <summary>Displays lose letters to the console.</summary>
    private static void DisplayLoseLetter()
    {
        string prompt = $@"
          ██╗   ██╗ ██████╗ ██╗   ██╗    ██╗      ██████╗ ███████╗███████╗        
          ╚██╗ ██╔╝██╔═══██╗██║   ██║    ██║     ██╔═══██╗██╔════╝██╔════╝        
           ╚████╔╝ ██║   ██║██║   ██║    ██║     ██║   ██║███████╗█████╗          
            ╚██╔╝  ██║   ██║██║   ██║    ██║     ██║   ██║╚════██║██╔══╝          
             ██║   ╚██████╔╝╚██████╔╝    ███████╗╚██████╔╝███████║███████╗        
             ╚═╝    ╚═════╝  ╚═════╝     ╚══════╝ ╚═════╝ ╚══════╝╚══════╝        
╔════════════════════════════════════════════════════════════════════════════════╗
║ Dear former CEO,                                                               ║
║                                                                                ║
║ We regret to inform you that you are being removed from the position of CEO    ║
║ and fired from the company, effective immediately. The board of directors of   ║
║ Oligopoly Investments has decided to take this action because you have spent   ║
║ the budget allocated to you, and your investment turned out to be unprofitable ║
║ for the company. We appreciate your service and wish you all the best in your  ║
║ future endeavors.                                                              ║
║                                                                                ║
║ Sincerely,                                                                     ║
║ The board of directors of Oligopoly Investments                                ║
╚════════════════════════════════════════════════════════════════════════════════╝
                                                                                  
Your Net Worth dropped below {LosingNetWorth}$
You have played {s_turnCounter} turns
Better luck next time...
";
        string[] options = ["Continue"];
        Menu winMenu = new(prompt, options);
        winMenu.RunMenu();
    }
    #endregion
}