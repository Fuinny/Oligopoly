using System.Xml.Serialization;

namespace Oligopoly.Source
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
                    RunSkipMenu();
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
        /// Runs skip menu.
        /// </summary>
        private static void RunSkipMenu()
        {
            string prompt = @"Welcome to Oligopoly!
Do you want to read the introductory letter or do you want to jump right into the gameplay?

";
            string[] options = { "Read introductory letter", "Skip introductory letter" };

            Menu skipMenu = new Menu(prompt, options, 0);

            int selectedOption = skipMenu.RunMenu();

            switch (selectedOption) 
            {
                case 0:
                    RunStartMenu();
                    break;
                case 1:
                    RunGameMenu();
                    break;
            }
        }

        /// <summary>
        /// Runs the start menu of the game.
        /// </summary>
        private static void RunStartMenu()
        {
            string prompt = @"Dear, new CEO
    On behalf of the board of directors of Oligopoly Investments, we would like to congratulate you on becoming our new CEO. We are confident that you will lead our company to new heights of success and innovation.
    As CEO, you now have access to our exclusive internal software called Oligopoly, where you can track the latest news from leading companies and buy and sell their shares. This software will give you an edge over the competition and help you make important decisions for our company. To access the program, simply click the button at the bottom of this email.
    We look forward to working with you and supporting you in your new role.

Sincerely,
The board of directors of Oligopoly Investments

";
            string[] options = { "Get Access" };

            Menu startMenu = new Menu(prompt, options, 15);

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

                using (Stream stream = File.Open("Data\\Data.xml", FileMode.Open))
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
            int currentEvent;
            double money = 10000;
            bool endGame = false;

            // Create a Random class object to generate event.
            Random random = new Random();

            // Start of the game cycle.
            while (!endGame)
            {
                // Generate event for current turn.
                currentEvent = random.Next(0, data?.gameEvents?.Count ?? 0);

                // Determine current event's type.
                if (data?.gameEvents?[currentEvent].Type == "Positive")  // If current event is positive.
                {
                    foreach (var currentCompany in data?.gameCompanies ?? Enumerable.Empty<Company>())
                    {
                        if (currentCompany.Ticker == data?.gameEvents?[currentEvent]?.Target)
                        {
                            currentCompany.SharePrice = Math.Round(currentCompany.SharePrice + currentCompany.SharePrice * (data?.gameEvents[currentEvent]?.Effect ?? 0) / 100, 2);
                        }
                    }
                }
                else if (data?.gameEvents?[currentEvent].Type == "Negative")  // If current event is negative.
                {
                    foreach (var currentCompany in data?.gameCompanies ?? Enumerable.Empty<Company>())
                    {
                        if (currentCompany.Ticker == data?.gameEvents?[currentEvent]?.Target)
                        {
                            currentCompany.SharePrice = Math.Round(currentCompany.SharePrice - currentCompany.SharePrice * (data?.gameEvents[currentEvent]?.Effect ?? 0) / 100, 2);
                        }
                    }
                }

                string prompt = "\nUse up and down arrow keys to select an option: \n";
                string[] options = { "Skip", "Buy", "Sell", "More About Companies" };
                GameMenu gameMenu = new GameMenu(prompt, options, 0, currentEvent, money, data);

                int selectedOption = gameMenu.RunMenu();

                switch (selectedOption)
                {
                    case 0:
                        break;
                    case 1:
                        Console.Clear();
                        DisplayBuyMenu(ref money, data);
                        Console.Clear();
                        break;
                    case 2:
                        Console.Clear();
                        DisplaySellMenu(ref money, data);
                        Console.Clear();
                        break;
                    case 3:
                        Console.Clear();
                        DisplayMoreAboutCompaniesMenu(data);
                        Console.Clear();
                        break;
                }

                // Check for win or loss.
                if (money < 0)
                {
                    Console.Clear();

                    string message = @"Dear, former CEO
    We regret to inform you that you are being removed from the position of CEO and fired from the company, effective immediately.
    The board of directors of Oligopoly Investments has decided to take this action because you have spent the budget allocated to you, and your investment turned out to be unprofitable for the company.
    We appreciate your service and wish you all the best in your future endeavors.

Sincerely,
The board of directors of Oligopoly Investments

";
                    foreach (char symbol in message)
                    {
                        Thread.Sleep(15);
                        Console.Write(symbol);
                    }

                    endGame = true;
                    Console.WriteLine("Press any key to exit the game...");
                    Console.ReadKey();
                    RunMainMenu();
                }
                else if (money >= 50000)
                {
                    Console.Clear();

                    string message = @"Dear CEO,
    On behalf of the board of directors of Oligopoly Investments, we would like to express our gratitude and understanding for your decision to leave your post. You have been a remarkable leader and a visionary strategist, who played the stock market skillfully and increased our budget by five times. We are proud of your achievements and we wish you all the best in your future endeavors.
    As a token of our appreciation, we are pleased to inform you that the company will pay you a bonus of $1 million. You deserve this reward for your hard work and dedication. We hope you will enjoy it and remember us fondly.
    Thank you for your service and your contribution to Oligopoly Investments. You will be missed.

Sincerely,
The board of directors of Oligopoly Investments

";
                    foreach (char symbol in message)
                    {
                        Thread.Sleep(15);
                        Console.Write(symbol);
                    }

                    endGame = true;
                    Console.WriteLine("Press any key to exit the game...");
                    Console.ReadKey();
                    RunMainMenu();
                }
                else
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// Displays companies descriptions to the console.
        /// </summary>
        /// <param name="data">An Data class object, that contain information about companies and events.</param>
        private static void DisplayMoreAboutCompaniesMenu(Data? data)
        {
            foreach (var company in data?.gameCompanies ?? Enumerable.Empty<Company>())
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
            for (int i = 0; i < data?.gameCompanies?.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {data.gameCompanies[i].Name}");
            }

            // Ask user to select a company.
            int selectedCompanyIndex;

            do
            {
                Console.Write("Select a company: ");
            } while (!int.TryParse(Console.ReadLine(), out selectedCompanyIndex) || selectedCompanyIndex < 1 || selectedCompanyIndex > data?.gameCompanies?.Count);

            // Ask user to type amount of shares.
            int buyAmount;

            do
            {
                Console.Write("Enter amount of shares: ");
            } while (!int.TryParse(Console.ReadLine(), out buyAmount) || buyAmount < 1);

            // Buy shares.
            if (data != null && data.gameCompanies != null && data.gameCompanies.Count >= selectedCompanyIndex)
            {
                data.gameCompanies[selectedCompanyIndex - 1].ShareAmount += buyAmount;
            }
            money -= buyAmount * (data?.gameCompanies?[selectedCompanyIndex - 1]?.SharePrice ?? 0);

            // Confirm transaction.
            Console.WriteLine($"You have bought {buyAmount} shares of {data?.gameCompanies?[selectedCompanyIndex - 1]?.Name} company.");
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
            for (int i = 0; i < data?.gameCompanies?.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {data.gameCompanies[i].Name}");
            }

            // Ask user to select a company.
            int selectedCompanyIndex;

            do
            {
                Console.Write("Select a company: ");
            } while (!int.TryParse(Console.ReadLine(), out selectedCompanyIndex) || selectedCompanyIndex < 1 || selectedCompanyIndex > data?.gameCompanies?.Count);

            // Ask user to type amount of shares.
            int sellAmount;

            do
            {
                Console.Write("Enter amount of shares: ");
            } while (!int.TryParse(Console.ReadLine(), out sellAmount) || sellAmount < 1 || sellAmount < data?.gameCompanies?[selectedCompanyIndex - 1].ShareAmount);

            // Sell shares.
            if (data != null && data.gameCompanies != null && data.gameCompanies.Count >= selectedCompanyIndex && data.gameCompanies[selectedCompanyIndex - 1].ShareAmount - sellAmount >= 0)
            {
                data.gameCompanies[selectedCompanyIndex - 1].ShareAmount -= sellAmount;
                money += sellAmount * (data?.gameCompanies?[selectedCompanyIndex - 1]?.SharePrice ?? 0);
                Console.WriteLine($"You have sold {sellAmount} shares of {data?.gameCompanies?[selectedCompanyIndex - 1].Name} company.");
            }
            else
            {
                Console.WriteLine("Entered not a valid value");
            }

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