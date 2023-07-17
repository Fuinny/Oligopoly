using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Oligopoly.Game;

public class Program
{
    private static List<Company> Companies = new List<Company>();
    private static List<Event> Events = new List<Event>();
    private static List<Event> GlobalEvents = new List<Event>();
    private static int Difficulty;
    private static int GameMode;
    private static int TurnCounter = 1;
    private static int PositiveEventChance;
    private static decimal Money = 10000M;
    private static decimal NetWorth;
    private const decimal LosingNetWorth = 2000.00M;
    private const decimal WinningNetWorth = 50000.00M;

    /// <summary>
    /// Program entry point.
    /// </summary>
    public static void Main()
    {
        if (!Directory.Exists("Saves"))
        {
            Directory.CreateDirectory("Saves");
        }
        if (OperatingSystem.IsWindows())
        {
            Console.CursorVisible = false;
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
        }
        DisplayMainMenuScreen();
    }

    /// <summary>
    /// Reads .xml files and checks data from files for correctness.
    /// </summary>
    private static void LoadEmbeddedResources()
    {
        try
        {
            XDocument document;
            {
                document = XDocument.Load(Path.Combine("Data", "Companies.xml"));
                foreach (XElement companyElement in document.Root.Elements("Company"))
                {
                    Company currentCompany = new Company
                    {
                        Name = companyElement.Element("Name").Value,
                        Industry = companyElement.Element("Industry").Value,
                        SharePrice = decimal.Parse(companyElement.Element("SharePrice").Value),
                        Description = companyElement.Element("Description").Value
                    };

                    Companies.Add(currentCompany);
                }
            }
            {
                document = XDocument.Load(Path.Combine("Data", "Events.xml"));
                foreach (XElement eventElement in document.Root.Elements("Event"))
                {
                    Event currentEvent = new Event
                    {
                        Effect = int.Parse(eventElement.Element("Effect").Value),
                        Target = eventElement.Element("Target").Value,
                        Title = eventElement.Element("Title").Value,
                        Content = eventElement.Element("Content").Value
                    };

                    Events.Add(currentEvent);
                }
            }
            {
                document = XDocument.Load(Path.Combine("Data", "GlobalEvents.xml"));
                foreach (XElement eventElement in document.Root.Elements("GlobalEvent"))
                {
                    Event currentEvent = new Event
                    {
                        Effect = int.Parse(eventElement.Element("Effect").Value),
                        Target = "All",
                        Title = eventElement.Element("Title").Value,
                        Content = eventElement.Element("Content").Value
                    };

                    GlobalEvents.Add(currentEvent);
                }
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

    /// <summary>
    /// Saves the current stage of the game.
    /// </summary>
    private static void SaveGame()
    {
        int fileCounter = 1;
        string fileName = $"Save_{fileCounter}.xml";
        string filePath = Path.Combine("Saves", fileName);

        try
        {
            while (File.Exists(filePath))
            {
                fileName = $"Save_{fileCounter}.xml";
                filePath = Path.Combine("Saves", fileName);
                fileCounter++;
            }
            XDocument saveFile = new XDocument(
                new XElement("SaveFile",
                    new XElement("GameMode", GameMode),
                    new XElement("Difficulty", Difficulty),
                    new XElement("CurrentTurn", TurnCounter),
                    new XElement("Money", Money),
                    new XElement("SharePrices", Companies.Select(company => new XElement($"{company.Name.Replace(" ", "_")}", company.SharePrice))),
                    new XElement("BuyedShares", Companies.Select(company => new XElement($"{company.Name.Replace(" ", "_")}", company.NumberOfShares)))
                    )
                );
            saveFile.Save(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error! \nDetails: {ex.Message}");
            Console.WriteLine("Press any key to exit the menu...");
            Console.ReadKey(true);
        }

        Console.WriteLine($"\nYour file was successfully saved with the name {fileName}");
        Console.WriteLine("You can find all of your save files in Saves folder.");
        Console.WriteLine("\nPress any key to exit the menu...");
        Console.ReadKey(true);
    }

    /// <summary>
    /// Loads the game from already created saves.
    /// </summary>
    private static bool LoadGame()
    {
        string[] saveFiles = Directory.GetFiles("Saves", "*.xml");

        if (saveFiles.Length == 0)
        {
            Console.Clear();
            Console.WriteLine("No save files found.");
            Console.WriteLine("\nPress any key to exit the menu...");
            Console.ReadKey(true);
            return false;
        }

        Menu loadMenu = new Menu("Select file to load: ", saveFiles);
        int selectedFile = loadMenu.RunMenu();

        try
        {
            XDocument saveFile = XDocument.Load(saveFiles[selectedFile]);
            GameMode = int.Parse(saveFile.Element("SaveFile").Element("GameMode").Value);
            Difficulty = int.Parse(saveFile.Element("SaveFile").Element("Difficulty").Value);
            TurnCounter = int.Parse(saveFile.Element("SaveFile").Element("CurrentTurn").Value);
            Money = decimal.Parse(saveFile.Element("SaveFile").Element("Money").Value);
            var sharePrices = saveFile.Element("SaveFile").Element("SharePrices").Elements();
            var buyedShares = saveFile.Element("SaveFile").Element("BuyedShares").Elements();

            foreach (var companyElement in sharePrices)
            {
                string companyName = companyElement.Name.LocalName.Replace("_", " ");
                decimal sharePrice = decimal.Parse(companyElement.Value);

                Company company = Companies.FirstOrDefault(c => c.Name == companyName);
                if (company != null)
                {
                    company.SharePrice = sharePrice;
                }
            }
            foreach (var companyElement in buyedShares)
            {
                string companyName = companyElement.Name.LocalName.Replace("_", " ");
                int numberOfShares = int.Parse(companyElement.Value);

                Company company = Companies.FirstOrDefault(c => c.Name == companyName);
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

    /// <summary>
    /// Displays main menu to the console.
    /// </summary>
    private static void DisplayMainMenuScreen()
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
        string[] options = { "Play", "Load", "About", "Exit" };
        Menu mainMenu = new Menu(prompt, options);
        while (true)
        {
            switch (mainMenu.RunMenu())
            {
                case 0:
                    LoadEmbeddedResources();
                    DisplayGameSetupMenu(false);
                    GameLoop();
                    break;
                case 1:
                    LoadEmbeddedResources();
                    LoadGame();
                    DisplayGameSetupMenu(true);
                    GameLoop();
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
        string[] options = { "Save", "Load", "Exit" };
        Menu pauseMenu = new Menu(prompt, options);
        switch (pauseMenu.RunMenu(default, true))
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
            string[] difficultiesOptions = { "Easy", "Normal", "Hard" };
            string[] gameModesOptions = { "Standard", "Random", "Custom" };
            string[] difficultiesDescriptions = {
                "60% chance that the next market event will be positive",
                "50% chance that the next market event will be positive/negative",
                "60% change that the next market event will be negative"
            };
            string[] gameModesDescriptions = {
                "Just standard mode, nothing out of the ordinary",
                "Your money and company shares prices will be randomly generated",
                "You can set the starting amount of your money"
            };

            TurnCounter = 1;
            Menu setupMenu = new Menu(prompt, gameModesOptions);
            GameMode = setupMenu.RunMenu(gameModesDescriptions);
            setupMenu = new Menu(prompt, difficultiesOptions);
            Difficulty = setupMenu.RunMenu(difficultiesDescriptions);

            switch (GameMode)
            {
                case 1:
                    foreach (Company company in Companies)
                    {
                        company.SharePrice = Random.Shared.Next(100, 5001);
                    }
                    Money = Random.Shared.Next(1000, 30001);
                    break;
                case 2:
                    string moneyPrompt = "Select amount of money: ";
                    string[] moneyOptions = { "(+) Increase", "(-) Decrease", "Done" };
                    Menu moneyMenu = new Menu(moneyPrompt, moneyOptions);
                    Money = moneyMenu.RunMoneySetupMenu();
                    break;
            }
        }

        switch (Difficulty)
        {
            case 0: PositiveEventChance = 60; break;
            case 1: PositiveEventChance = 50; break;
            case 2: PositiveEventChance = 40; break;
        }
    }

    /// <summary>
    /// Runs a game session.
    /// </summary>
    private static void GameLoop()
    {
        bool isGameEnded = false;

        while (!isGameEnded)
        {
            CalculateNetWorth();

            StringBuilder prompt = Menu.DrawCompaniesTable(Companies);
            prompt.AppendLine($"\nYou have: {Math.Round(Money, 2)}$     Your Net Worth: {Math.Round(NetWorth, 2)}$     Current Turn: {TurnCounter}");
            string[] options = { "Wait For Market Change", "Buy", "Sell", "More About Companies" };

            Menu gameMenu = new Menu(prompt.ToString(), options);

            switch (gameMenu.RunMenu(default, true))
            {
                case -1:
                    DisplayPauseMenu();
                    continue;
                case 0:
                    UpdateMarketPrices();
                    GenerateEvent();
                    break;
                case 1:
                    DisplayBuyOrSellScreen(true);
                    break;
                case 2:
                    DisplayBuyOrSellScreen(false);
                    break;
                case 3:
                    DisplayMoreAboutCompaniesScreen();
                    break;
            }

            if (NetWorth > WinningNetWorth)
            {
                isGameEnded = true;
                DisplayWinLetter();
            }
            else if (NetWorth < LosingNetWorth)
            {
                isGameEnded = true;
                DisplayLoseLetter();
            }

            TurnCounter++;
        }

        Companies.Clear();
        Events.Clear();
    }

    /// <summary>
    /// Changes the share prices of all companies from (-)1 to (-)3 percent
    /// </summary>
    private static void UpdateMarketPrices()
    {
        for (int i = 0; i < Companies.Count; i++)
        {
            Random random = new Random();
            int effect = random.Next(0, 2);

            switch (effect)
            {
                case 0:
                    Companies[i].SharePrice += Companies[i].SharePrice * Random.Shared.Next(1, 4) / 100;
                    break;
                case 1:
                    Companies[i].SharePrice += Companies[i].SharePrice * Random.Shared.Next(-3, 0) / 100;
                    break;
            }
        }
    }

    /// <summary>
    /// Calculates current net worth.
    /// </summary>
    private static void CalculateNetWorth()
    {
        NetWorth = Money;
        foreach (Company company in Companies)
        {
            NetWorth += company.NumberOfShares * company.SharePrice;
        }
    }

    /// <summary>
    /// Generates a random event.
    /// </summary>
    private static void GenerateEvent()
    {
        Event currentEvent;
        StringBuilder prompt = Menu.DrawCompaniesTable(Companies);

        if (TurnCounter % 50 == 0)
        {
            currentEvent = GlobalEvents[Random.Shared.Next(0, GlobalEvents.Count)];
            foreach (Company currentCompany in Companies)
            {
                currentCompany.SharePrice += currentCompany.SharePrice * currentEvent.Effect / 100;
            }
            prompt.AppendLine($"\n{currentEvent.Title.ToUpper()}");
        }
        else
        {
            currentEvent = Events[Random.Shared.Next(0, Events.Count)];
            foreach (Company currentCompany in Companies)
            {
                if (currentCompany.Name == currentEvent.Target)
                {
                    currentCompany.SharePrice += currentCompany.SharePrice * currentEvent.Effect / 100;
                }
            }
            prompt.AppendLine($"\n{currentEvent.Title}");
        }

        prompt.AppendLine($"\n{currentEvent.Content}");
        string[] options = { "Continue" };
        Menu eventMenu = new Menu(prompt.ToString(), options);
        eventMenu.RunMenu();
    }

    /// <summary>
    /// Displays buy or sell menu.
    /// </summary>
    /// <param name="isBuying">A flag that specifies how the method should work. True - buy. False - sell.</param>
    public static void DisplayBuyOrSellScreen(bool isBuying)
    {
        StringBuilder prompt = Menu.DrawCompaniesTable(Companies);
        prompt.AppendLine($"\nYou have: {Math.Round(Money, 2)}$");
        prompt.AppendLine($"\nUse the arrow keys and enter to confirm how many shares to {(isBuying ? "buy" : "sell")}:");
        int[] numberOfSharesToProcess = new int[Companies.Count];
        string[] options = new string[Companies.Count];
        for (int i = 0; i < Companies.Count; i++)
        {
            options[i] = Companies[i].Name;
        }
        Menu buyOrSellMenu = new Menu(prompt.ToString(), options);
        buyOrSellMenu.RunBuyOrSellMenu(ref numberOfSharesToProcess, Companies, Money, isBuying);

        for (int i = 0; i < numberOfSharesToProcess.Length; i++)
        {
            if (isBuying)
            {
                Money -= numberOfSharesToProcess[i] * Companies[i].SharePrice;
                Companies[i].NumberOfShares += numberOfSharesToProcess[i];
            }
            else
            {
                Money += numberOfSharesToProcess[i] * Companies[i].SharePrice;
                Companies[i].NumberOfShares -= numberOfSharesToProcess[i];
            }
        }
    }

    /// <summary>
    /// Displays companies descriptions to the console.
    /// </summary>
    private static void DisplayMoreAboutCompaniesScreen()
    {
        StringBuilder prompt = new StringBuilder();
        foreach (Company company in Companies)
        {
            prompt.AppendLine($"{company.Name} - {company.Description}");
            prompt.AppendLine();
        }
        string[] options = { "Continue" };
        Menu aboutCompaniesMenu = new Menu(prompt.ToString(), options);
        aboutCompaniesMenu.RunMenu();
    }

    /// <summary>
    /// Displays introduction letter.
    /// </summary>
    private static void DisplayIntroductionLetter()
    {
        string prompt = @"
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
        string[] options = { "Start the Game" };
        Menu introductionMenu = new Menu(prompt, options);
        introductionMenu.RunMenu();
    }

    /// <summary>
    /// Displays win letter.
    /// </summary>
    private static void DisplayWinLetter()
    {
        string prompt = @$"
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
You have played {TurnCounter} turns
You win! Congratulations!
";
        string[] options = { "Return to Main Menu" };
        Menu winMenu = new Menu(prompt, options);
        winMenu.RunMenu();
    }

    /// <summary>
    /// Displays lose letter.
    /// </summary>
    private static void DisplayLoseLetter()
    {
        string prompt = @$"
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
You have played {TurnCounter} turns
You Lose! Better luck next time...
";
        string[] options = { "Return to Main Menu" };
        Menu loseMenu = new Menu(prompt, options);
        loseMenu.RunMenu();
    }

    /// <summary>
    /// Displays thanks to the player and information about the creator of the game.
    /// </summary>
    private static void DisplayAboutGameMenu()
    {
        string prompt = @"
╔══════════════════════════════════════════════════════════════════════════════════╗
║THANKS!                                                                           ║
║                                                                                  ║
║No really, thank you for taking time to play this simple CLI game. It means a lot.║
║If you find any bug or have an idea how to improve the game, please let me know :D║
║                                                                                  ║
║This game was created by Semion Medvedev (Fuinny)                                 ║
╚══════════════════════════════════════════════════════════════════════════════════╝
";
        string[] options = { "Return to Main Menu" };
        Menu aboutGameMenu = new Menu(prompt, options);
        aboutGameMenu.RunMenu();
    }

    /// <summary>
    /// Displays exit menu.
    /// </summary>
    private static void DisplayExitMenu()
    {
        string prompt = "Are you sure you want to exit the game?";
        string[] options = { "Exit the Game", "Back" };
        Menu exitMenu = new Menu(prompt, options);
        switch (exitMenu.RunMenu())
        {
            case 0:
                Environment.Exit(0);
                break;
        }
    }
}