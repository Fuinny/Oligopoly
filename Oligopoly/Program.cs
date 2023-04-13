namespace Oligopoly
{
    public class Program
    {
        /// <summary>
        ///  Program entry point.
        /// </summary>
        /// <param name="args"></param>
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

            Menu mainMenu = new Menu(prompt, options);

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