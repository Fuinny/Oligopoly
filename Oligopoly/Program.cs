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
        /// Runs game menu.
        /// </summary>
        private static void RunGameMenu()
        {
            Data? data = new Data();

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Data));

                using (StringReader reader = new StringReader(File.ReadAllText("Data.xml")))
                {
                    data = (Data?)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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