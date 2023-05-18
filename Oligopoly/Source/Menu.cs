using System;
using System.Text;
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

        public int RunMenu()
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

        public static StringBuilder DrawCompaniesTable(List<Company> companies)
        {
            const int c0 = 30, c1 = 10, c2 = 20, c3 = 15;

            StringBuilder companiesTable = new StringBuilder();
            companiesTable.AppendLine($"╔═{new('═', c0)}═╦═{new('═', c1)}═╦═{new('═', c2)}═╦═{new('═', c3)}═╗");
            companiesTable.AppendLine($"║ {"Company", -c0} ║ {"Industry", c1} ║ {"Share Price", c2} ║ {"You Have", c3} ║");
            companiesTable.AppendLine($"╠═{new('═', c0)}═╬═{new('═', c1)}═╬═{new('═', c2)}═╬═{new('═', c3)}═╣");
            foreach (Company company in companies) 
            {
                companiesTable.AppendLine($"║ {company.Name, -c0} ║ {company.Industry, c1} ║ {Math.Round(company.SharePrice, 3), c2} ║ {company.NumberShares, c3} ║");
            }
            companiesTable.AppendLine($"╚═{new('═', c0)}═╩═{new('═', c1)}═╩═{new('═', c2)}═╩═{new('═', c3)}═╝");

            return companiesTable;
        }

        public static StringBuilder DrawCompaniesTableWithEvent(List<Company> companies, Event currentEvent)
        {
            const int c0 = 30, c1 = 10, c2 = 20, c3 = 15;

            StringBuilder companiesTable = new StringBuilder();
            companiesTable.AppendLine($"╔═{new('═', c0)}═╦═{new('═', c1)}═╦═{new('═', c2)}═╦═{new('═', c3)}═╗");
            companiesTable.AppendLine($"║ {"Company",-c0} ║ {"Industry",c1} ║ {"Share Price",c2} ║ {"You Have",c3} ║");
            companiesTable.AppendLine($"╠═{new('═', c0)}═╬═{new('═', c1)}═╬═{new('═', c2)}═╬═{new('═', c3)}═╣");
            foreach (Company company in companies)
            {
                companiesTable.AppendLine($"║ {company.Name,-c0} ║ {company.Industry,c1} ║ {Math.Round(company.SharePrice, 3), c2} ║ {company.NumberShares,c3} ║");
            }
            companiesTable.AppendLine($"╚═{new('═', c0)}═╩═{new('═', c1)}═╩═{new('═', c2)}═╩═{new('═', c3)}═╝");
            companiesTable.AppendLine($"\n{currentEvent.Title}");
            companiesTable.AppendLine($"\n{currentEvent.Content}");

            return companiesTable;
        }
    }
}
