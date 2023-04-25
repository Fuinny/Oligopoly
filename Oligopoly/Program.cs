using System.Xml;
using System.Xml.Serialization;

namespace Oligopoly
{
    public class Program
    {
        /// <summary>
        ///  Program entry point.
        /// </summary>
        /// <param name="args">The array of strings to process.</param>
        public static void Main(string[] args)
        {
            RunMainMenu();
        }

        /// <summary>
        /// Runs the main menu of the game.
        /// </summary>
        private static void RunMainMenu()
        {
            string prompt = @"
 ██████╗ ██╗     ██╗ ██████╗  ██████╗ ██████╗  ██████╗ ██╗  ██╗   ██╗
██╔═══██╗██║     ██║██╔════╝ ██╔═══██╗██╔══██╗██╔═══██╗██║  ╚██╗ ██╔╝
██║   ██║██║     ██║██║  ███╗██║   ██║██████╔╝██║   ██║██║   ╚████╔╝ 
██║   ██║██║     ██║██║   ██║██║   ██║██╔═══╝ ██║   ██║██║    ╚██╔╝  
╚██████╔╝███████╗██║╚██████╔╝╚██████╔╝██║     ╚██████╔╝███████╗██║   
 ╚═════╝ ╚══════╝╚═╝ ╚═════╝  ╚═════╝ ╚═╝      ╚═════╝ ╚══════╝╚═╝   
        --Use up and down arrow keys to select an option--                                                                     
";
            string[] options = { "Play", "About", "Exit" };

            Menu mainMenu = new Menu(prompt, options, 0);

            int selectedOption = mainMenu.RunMenu();

            switch (selectedOption) 
            {
                case 0:
                    RunStartMenu();
                    break;
                case 1:
                    DisplayAboutInfo();
                    break;
                case 2:
                    ExitGame();
                    break;
            }
        }

        /// <summary>
        /// Runs the start menu of the game.
        /// </summary>
        private static void RunStartMenu()
        {
            string prompt = @"    On behalf of directors of Oligopoly Investments we would like to congratulate you on taking office as new CEO of our company.
    We are confident that you have the vision, skills and experience to lead us to greater success and growth. As new CEO, you now have access to our company's internal software - Oligopoly. This software a modern and powerful tool, that will help you analyze market trends and opportunities. To access the program, simply click the button at the bottom of this email. 
    We look forward to working with you and supporting you in your new role. Please do not hesitate to contact us if you have any questions or concerns.

Sincerely,
The Board of Directors

";
            string[] options = { "Get Access" };

            Menu startMenu = new Menu(prompt, options, 30);

            int selectedOption = startMenu.RunMenu();

            switch (selectedOption) 
            {
                case 0:
                    Console.Clear();
                    RunGameMenu();
                    break;
            }
        }

        /// <summary>
        /// Runs the game menu.
        /// </summary>
        private static void RunGameMenu()
        {
            // Create a Data class object, that contains game companies and events.
            Data? data = new Data();

            // Read the .xml file.
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Data));

                using (Stream stream = File.Open("Data.xml", FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        data = (Data?)serializer.Deserialize(reader);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error! In file Data.xml specified invalid value.");

                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error! \nDetails: {ex.Message}");
            }

            // Create variables.
            double money = 10000;
            int currentEvent;

            // Create a Random class object to generate event.
            Random random = new Random();

            // Start of the game cycle.
            while (money > 0)
            {  
                // Generate event for current turn.
                currentEvent = random.Next(0, data?.gameEvents?.Count?? 0);

                // Determine current event's type.
                if (data?.gameEvents?[currentEvent].Type == "Positive")  // If current event is positive.
                {
                    foreach (var currentCompany in data.gameCompanies)
                    {
                        if (currentCompany.Ticker == data.gameEvents[currentEvent].Target)
                        {
                            currentCompany.SharePrice = Math.Round(currentCompany.SharePrice + currentCompany.SharePrice * data.gameEvents[currentEvent].Effect / 100, 2);
                        }
                    }
                }
                else if (data?.gameEvents?[currentEvent].Type == "Negative")  // If current event is negative.
                {
                    foreach (var currentCompany in data.gameCompanies)
                    {
                        if (currentCompany.Ticker == data.gameEvents[currentEvent].Target)
                        {
                            currentCompany.SharePrice = Math.Round(currentCompany.SharePrice - currentCompany.SharePrice * data.gameEvents[currentEvent].Effect / 100, 2);
                        }
                    }
                }

                string prompt = "\nUse up and down arrow keys to select an option: \n";
                string[] options = { "Buy", "Sell", "Skip", "More About Companies" };
                GameMenu gameMenu = new GameMenu(prompt, options, 0, currentEvent, money, data);

                int selectedOption = gameMenu.RunMenu();

                switch (selectedOption)
                {
                    case 0:
                        Console.Clear();
                        DisplayBuyMenu(ref money, data);
                        Console.Clear();
                        break;
                    case 1:
                        Console.Clear();
                        DisplaySellMenu(ref money, data);
                        Console.Clear();
                        break;
                    case 2:
                        continue;
                    case 3:
                        Console.Clear();
                        DisplayMoreAboutCompaniesMenu(data);
                        Console.Clear();
                        break;
                }
            }
        }

        /// <summary>
        /// Displays companies descriptions to the console.
        /// </summary>
        /// <param name="data">An Data class object, that contain information about companies and events.</param>
        private static void DisplayMoreAboutCompaniesMenu(Data? data)
        {
            foreach (var company in data.gameCompanies)
            {
                Console.WriteLine($"{company.Name} - {company.Description}\n");
            }

            Console.WriteLine("Press any key to exit the menu...");
            Console.ReadKey();
        }

        /// <summary>
        /// Displays buy menu.
        /// </summary>
        /// <param name="money">The amount of money the user has.</param>
        /// <param name="data">An Data class object, that contain information about companies and events.</param>
        private static void DisplayBuyMenu(ref double money, Data? data)
        {
            // Display all game companies.
            for (int i = 0; i < data.gameCompanies.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {data.gameCompanies[i].Name}");
            }

            // Ask user to select a company.
            int selectedCompanyIndex;

            do
            {
                Console.Write("Select a company: ");
            } while (!int.TryParse(Console.ReadLine(), out selectedCompanyIndex) || selectedCompanyIndex < 1 || selectedCompanyIndex > data.gameCompanies.Count);

            // Ask user to type amount of shares.
            int buyAmount;

            do
            {
                Console.Write("Enter amount of shares: ");
            } while (!int.TryParse(Console.ReadLine(), out buyAmount) || buyAmount < 1);

            // Buy shares.
            data.gameCompanies[selectedCompanyIndex - 1].ShareAmount += buyAmount;
            money = money - (buyAmount * data.gameCompanies[selectedCompanyIndex - 1].SharePrice);

            // Confirm transaction.
            Console.WriteLine($"You have bought {buyAmount} shares of {data.gameCompanies[selectedCompanyIndex - 1].Name} company.");
            Console.WriteLine("Press any key to exit the menu...");
            Console.ReadKey();
        }

        /// <summary>
        /// Displays sell menu.
        /// </summary>
        /// <param name="money">The amount of money the user has. </param>
        /// <param name="data">An Data class object, that contain information about companies and events.</param>
        private static void DisplaySellMenu(ref double money, Data? data)
        {
            // Display all game companies.
            for (int i = 0; i < data.gameCompanies.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {data.gameCompanies[i].Name}");
            }

            // Ask user to select a company.
            int selectedCompanyIndex;

            do
            {
                Console.Write("Select a company: ");
            } while (!int.TryParse(Console.ReadLine(), out selectedCompanyIndex) || selectedCompanyIndex < 1 || selectedCompanyIndex > data.gameCompanies.Count);

            // Ask user to type amount of shares.
            int sellAmount;

            do
            {
                Console.Write("Enter amount of shares: ");
            } while (!int.TryParse(Console.ReadLine(), out sellAmount) || sellAmount < 1 || sellAmount < data.gameCompanies[selectedCompanyIndex - 1].ShareAmount);

            // Buy shares.
            data.gameCompanies[selectedCompanyIndex - 1].ShareAmount -= sellAmount;
            money = money + (sellAmount * data.gameCompanies[selectedCompanyIndex - 1].SharePrice);

            // Confirm transaction.
            Console.WriteLine($"You have sold {sellAmount} shares of {data.gameCompanies[selectedCompanyIndex - 1].Name} company.");
            Console.WriteLine("Press any key to exit the menu...");
            Console.ReadKey();
        }

        /// <summary>
        /// Displays information about the game.
        /// </summary>
        private static void DisplayAboutInfo()
        {
            Console.Clear();
            Console.WriteLine(@"THANKS!
No really, thank you for taking time to play this simple console game. It means a lot.

This game was created by Semion Medvedev (Fuinny)
My GitHub profile: https://github.com/Fuinny

Press any key to exit the menu...");
            Console.ReadKey(true);
            RunMainMenu();
        }

        /// <summary>
        /// Exits the game.
        /// </summary>
        private static void ExitGame()
        {
            Console.Clear();
            Console.WriteLine("Press any key to exit the game...");
            Console.ReadKey(true);
            Environment.Exit(0);
        }
    }
}