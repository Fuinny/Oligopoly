using System;
using System.Runtime.CompilerServices;
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
                switch (mainMenu.RunGenericMenu())
                {
                    case 0:
                        DisplayDifficultiesScreen();
                        InitializeGame();
                        // Show introductory letter.
                        // Run game loop.
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
            string[] options = { "Easy", "Normal", "Hard"};
            Menu difficultiesMenu = new Menu(prompt, options);
            switch (difficultiesMenu.RunGenericMenu())
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
                case "difficult":
                    Money = 5000.00M;
                    LosingNetWorth = 3000.00M;
                    WinningNetWorth = 100000.00M;
                    break;
            }
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
            aboutGameMenu.RunGenericMenu();
        }

        private static void DisplayExitMenu()
        {
            string prompt = "Are you sure you want to exit the game?";
            string[] options = { "Exit the Game", "Back" };
            Menu exitMenu = new Menu(prompt, options);
            switch (exitMenu.RunGenericMenu())
            {
                case 0:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}