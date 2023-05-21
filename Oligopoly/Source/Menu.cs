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

        public int RunMenu()
        {
            ConsoleKey keyPressed;

            do
            {
                Console.Clear();
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
                        if (SelectedIndex > Options.Length - 1)
                        {
                            SelectedIndex = 0;
                        }
                        break;
                }
            } while (keyPressed != ConsoleKey.Enter);

            return SelectedIndex;
        }

        public int RunMenuWithDescription(string[] descriptions)
        {
            ConsoleKey keyPressed;

            do
            {
                Console.Clear();
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

                Console.WriteLine("\nDescription:");
                Console.WriteLine(descriptions[SelectedIndex]);

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
                        if (SelectedIndex > Options.Length - 1)
                        {
                            SelectedIndex = 0;
                        }
                        break;
                }
            } while (keyPressed != ConsoleKey.Enter);

            return SelectedIndex;
        }

        public void RunBuyOrSellMenu(ref int[] numberOfSharesToProcess, List<Company> companies, decimal money, bool isBuying)
        {
            ConsoleKey keyPressed;

            do
            {
                Console.Clear();
                Console.WriteLine(Prompt);

                decimal transactionCost = 0.0M;
                for (int i = 0; i < numberOfSharesToProcess.Length; i++)
                {
                    transactionCost += numberOfSharesToProcess[i] * companies[i].SharePrice;
                }

                for (int i = 0; i < Options.Length; i++)
                {
                    if (i == SelectedIndex)
                    {
                        (Console.ForegroundColor, Console.BackgroundColor) = (Console.BackgroundColor, Console.ForegroundColor);
                        Console.WriteLine($"[*] <{numberOfSharesToProcess[i]}> {Options[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"[ ] <{numberOfSharesToProcess[i]}> {Options[i]}");
                    }
                }

                Console.WriteLine($"\nTransaction amount: {Math.Round(transactionCost, 2)}$");

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
                        if (SelectedIndex > Options.Length - 1)
                        {
                            SelectedIndex = 0;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (numberOfSharesToProcess[SelectedIndex] > 0)
                        {
                            numberOfSharesToProcess[SelectedIndex]--;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (isBuying)
                        {
                            if (transactionCost + companies[SelectedIndex].SharePrice <= money)
                            {
                                numberOfSharesToProcess[SelectedIndex]++;
                            }
                        }
                        else
                        {
                            if (numberOfSharesToProcess[SelectedIndex] < companies[SelectedIndex].NumberShares)
                            {
                                numberOfSharesToProcess[SelectedIndex]++;
                            }
                        }
                        break;
                }

            } while (keyPressed != ConsoleKey.Enter);

            Console.Clear();
            Console.WriteLine(Prompt);
            Console.WriteLine("\nTransaction completed.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }

        public static StringBuilder DrawCompaniesTable(List<Company> companies)
        {
            const int c0 = 30, c1 = 10, c2 = 20, c3 = 15;

            StringBuilder companiesTable = new StringBuilder();
            companiesTable.AppendLine($"╔═{new('═', c0)}═╦═{new('═', c1)}═╦═{new('═', c2)}═╦═{new('═', c3)}═╗");
            companiesTable.AppendLine($"║ {"Company",-c0} ║ {"Industry",c1} ║ {"Share Price",c2} ║ {"You Have",c3} ║");
            companiesTable.AppendLine($"╠═{new('═', c0)}═╬═{new('═', c1)}═╬═{new('═', c2)}═╬═{new('═', c3)}═╣");
            foreach (Company company in companies)
            {
                companiesTable.AppendLine($"║ {company.Name,-c0} ║ {company.Industry,c1} ║ {Math.Round(company.SharePrice, 2),c2} ║ {company.NumberShares,c3} ║");
            }
            companiesTable.AppendLine($"╚═{new('═', c0)}═╩═{new('═', c1)}═╩═{new('═', c2)}═╩═{new('═', c3)}═╝");

            return companiesTable;
        }
    }
}
