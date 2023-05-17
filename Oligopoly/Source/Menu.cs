using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Oligopoly
{
    public class Menu
    {
        private int SelectedIndex;
        private string Prompt;
        private string[] Options;

        public Menu(string prompt, string[] options) 
        {
            Prompt = prompt;
            Options = options;
            SelectedIndex = 0;
        }

        private void UpdateMenu()
        {
            Console.WriteLine(Prompt);

            for (int i = 0; i < Options.Length; i++)
            {
                if (i == SelectedIndex)
                {
                    (Console.ForegroundColor, Console.BackgroundColor) = (Console.BackgroundColor, Console.ForegroundColor);
                    Console.WriteLine($"[*] {Options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"[ ] {Options[i]}");
                }
            }
        }

        public int RunGenericMenu()
        {
            ConsoleKey keyPressed;

            do
            {
                Console.Clear();
                UpdateMenu();

                ConsoleKeyInfo keyInfo = Console.ReadKey();
                keyPressed = keyInfo.Key;

                switch (keyPressed)
                {
                    case ConsoleKey.UpArrow:
                        SelectedIndex--;
                        if (SelectedIndex == -1)
                        {
                            SelectedIndex = Options.Length - 1;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        SelectedIndex++;
                        if (SelectedIndex > Options.Length -1)
                        {
                            SelectedIndex = 0;
                        }
                        break;
                }
            } while (keyPressed != ConsoleKey.Enter);

            return SelectedIndex;
        }
    }
}
