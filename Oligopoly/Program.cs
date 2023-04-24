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

            Menu startMenu = new Menu(prompt, options, 0);

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
            int currentEvent, currentTurn;

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

                //string prompt = "\nUse up and down arrow keys to select an option: \n";
                //string[] options = { "Buy", "Sell", "Skip", "More About Companies" };
                //GameMenu gameMenu = new GameMenu(prompt, options, 0, currentEvent, data);

                //int selectedOption = gameMenu.RunMenu();

                //switch (selectedOption)
                //{
                //    case 0:
                //        break;
                //    case 1:
                //        break;
                //    case 2:
                //        break;
                //    case 3:
                //        break;
                //}
            }
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