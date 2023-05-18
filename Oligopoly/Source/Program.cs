using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Oligopoly
{
    public class Program
    {
        private static List<Company> Companies = new List<Company>();
        private static List<Event> Events = new List<Event>();
        private static string Difficulty;
        private static decimal Money;
        private static decimal NetWorth;
        private static decimal LosingNetWorth;
        private static decimal WinningNetWorth;

        public static void Main(string[] args)
        {
            Console.CursorVisible = false;
            LoadEmbeddedResources();
            DisplayMainMenuScreen();
        }

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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error! \nDetails: {ex.Message}");
                Console.WriteLine("Press any key to exit the game...");
                Console.ReadKey(true);
                Environment.Exit(0);
            }
        }

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
            string[] options = { "Play", "About", "Exit" };
            Menu mainMenu = new Menu(prompt, options);
            while (true)
            {
                switch (mainMenu.RunMenu())
                {
                    case 0:
                        DisplayDifficultiesScreen();
                        InitializeGame();
                        DisplayIntroductionLetter();
                        GameLoop();
                        break;
                    case 1:
                        DisplayAboutGameMenu();
                        break;
                    case 2:
                        DisplayExitMenu();
                        break;
                }
            }
        }
        private static void DisplayDifficultiesScreen()
        {
            string prompt = "Select difficulty: ";
            string[] options = { "Easy", "Normal", "Hard" };
            Menu difficultiesMenu = new Menu(prompt, options);
            switch (difficultiesMenu.RunMenu())
            {
                case 0:
                    Difficulty = "easy";
                    break;
                case 1:
                    Difficulty = "normal";
                    break;
                case 2:
                    Difficulty = "hard";
                    break;
            }
        }

        private static void InitializeGame()
        {
            switch (Difficulty)
            {
                case "easy":
                    Money = 20000.00M;
                    LosingNetWorth = 1000.00M;
                    WinningNetWorth = 30000.00M;
                    break;
                case "normal":
                    Money = 10000.00M;
                    LosingNetWorth = 2000.00M;
                    WinningNetWorth = 50000.00M;
                    break;
                case "hard":
                    Money = 5000.00M;
                    LosingNetWorth = 3000.00M;
                    WinningNetWorth = 100000.00M;
                    break;
            }
        }

        private static void GameLoop()
        {
            bool isGameEnded = false;

            while (!isGameEnded)
            {
                CalculateNetWorth();
                StringBuilder prompt = Menu.DrawCompaniesTable(Companies);
                prompt.AppendLine($"\nYou have: {Math.Round(Money, 2)}$     Your Net Worth: {Math.Round(NetWorth, 2)}$");
                string[] options = { "Wait For Market Change", "Buy", "Sell", "More About Companies" };
                Menu gameMenu = new Menu(prompt.ToString(), options);
                switch (gameMenu.RunMenu())
                {
                    case 0:
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
            }
        }

        private static void GenerateEvent()
        {
            Event currentEvent = Events[Random.Shared.Next(0, Events.Count)];

            foreach (Company currentCompany in Companies)
            {
                if (currentCompany.Name == currentEvent.Target)
                {
                    currentCompany.SharePrice += currentCompany.SharePrice * currentEvent.Effect / 100;
                }
            }

            StringBuilder prompt = Menu.DrawCompaniesTable(Companies);
            prompt.AppendLine($"\n{currentEvent.Title}");
            prompt.AppendLine($"\n{currentEvent.Content}");
            string[] options = { "Continue" };
            Menu eventMenu = new Menu(prompt.ToString(), options);
            eventMenu.RunMenu();
        }

        public static void DisplayBuyOrSellScreen(bool isBuying)
        {
            StringBuilder prompt = Menu.DrawCompaniesTable(Companies);
            prompt.AppendLine($"\nYou have: {Money}$");
            prompt.AppendLine("\nUse an arrow keys to select company and amount of shares. Press enter to confirm: ");
            int[] numberOfSharesToProcess = new int[Companies.Count];
            string[] options = new string[Companies.Count];
            for (int i = 0; i < Companies.Count; i++)
            {
                options[i] = Companies[i].Name;
            }
            Menu buyOrSellMenu = new Menu(prompt.ToString(), options);
            buyOrSellMenu.RunBuyOrSellMenu(ref numberOfSharesToProcess, Companies, Money);

            for (int i = 0; i < numberOfSharesToProcess.Length; i++)
            {
                if (isBuying)
                {
                    Money -= numberOfSharesToProcess[i] * Companies[i].SharePrice;
                    Companies[i].NumberShares += numberOfSharesToProcess[i];
                }
                else
                {
                    Money += numberOfSharesToProcess[i] * Companies[i].SharePrice;
                    Companies[i].NumberShares -= numberOfSharesToProcess[i];
                }
            }
        }

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

        private static void CalculateNetWorth()
        {
            NetWorth = Money;
            foreach (Company company in Companies)
            {
                NetWorth += company.NumberShares * company.SharePrice;
            }
        }

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

        private static void DisplayWinLetter()
        {

        }

        private static void DisplayLoseLetter()
        {

        }

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
}