namespace Oligopoly.Game;

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

    /// <summary>
    /// Displays and updates menu with given prompt and options.
    /// </summary>
    /// <param name="descriptions">An array of descriptions for each menu option.</param>
    /// <param name="isPausable">Determines if the pause menu can be called from this menu.</param>
    /// <returns>An integer that represents selected option.</returns>
    public int RunMenu(string[] descriptions = default, bool isPausable = false)
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

            if (descriptions != null)
            {
                Console.WriteLine("\nDescription:");
                Console.WriteLine(descriptions[SelectedIndex]);
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            keyPressed = keyInfo.Key;

            if (isPausable)
            {
                switch (keyPressed)
                {
                    case ConsoleKey.P:
                        return -1;
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
            }
            else
            {
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
            }
        } while (keyPressed != ConsoleKey.Enter);

        return SelectedIndex;
    }

    /// <summary>
    /// Displays and updates money setup menu with given prompt and options.
    /// </summary>
    /// <returns>A decimal value that represents selected amount of money.</returns>
    public decimal RunMoneySetupMenu()
    {
        decimal customMoney = 1000M;
        ConsoleKey keyPressed;

        while (true)
        {
            Console.Clear();
            Console.WriteLine(Prompt);
            Console.WriteLine($"\nYour starting amount of money will be set to {customMoney:C}");
            Console.WriteLine("Use up and down arrow keys and enter to select an option:\n");

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
                case ConsoleKey.Enter:
                    if (SelectedIndex == 0)
                    {
                        if (customMoney + 100 <= 45000)
                        {
                            customMoney += 100;
                        }
                    }
                    else if (SelectedIndex == 1)
                    {
                        if (customMoney - 100 >= 1000)
                        {
                            customMoney -= 100;
                        }
                    }
                    else
                    {
                        return customMoney;
                    }
                    break;
            }

        }
    }

    /// <summary>
    /// Displays and updates buy or sell menu with given prompt and options.
    /// </summary>
    /// <param name="numberOfSharesToProcess">Number of shares that player buy or sell.</param>
    /// <param name="companies">List of all in-game companies.</param>
    /// <param name="money">Current amount of player's money.</param>
    /// <param name="isBuying">Determines if the player is buying or selling shares.</param>
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
                    Console.WriteLine($"[*] <{numberOfSharesToProcess[i]}{(isBuying ? "" : $"/{companies[i].NumberOfShares}")}> {Options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"[ ] <{numberOfSharesToProcess[i]}> {Options[i]}");
                }
            }

            Console.WriteLine($"\nTransaction {(isBuying ? "cost" : "payout")}: {Math.Round(transactionCost, 2):C}");

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
                        if (numberOfSharesToProcess[SelectedIndex] < companies[SelectedIndex].NumberOfShares)
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

    /// <summary>
    /// Draws table with information about all in-game companies.
    /// </summary>
    /// <param name="companies">List of all in-game companies.</param>
    /// <returns>A StringBuilder object that contains table.</returns>
    public static StringBuilder DrawCompaniesTable(List<Company> companies)
    {
        const int c0 = 30, c1 = 10, c2 = 20, c3 = 15;

        StringBuilder companiesTable = new StringBuilder();
        companiesTable.AppendLine($"╔═{new('═', c0)}═╦═{new('═', c1)}═╦═{new('═', c2)}═╦═{new('═', c3)}═╗");
        companiesTable.AppendLine($"║ {"Company",-c0} ║ {"Industry",c1} ║ {"Share Price",c2} ║ {"You Have",c3} ║");
        companiesTable.AppendLine($"╠═{new('═', c0)}═╬═{new('═', c1)}═╬═{new('═', c2)}═╬═{new('═', c3)}═╣");
        foreach (Company company in companies)
        {
            companiesTable.AppendLine($"║ {company.Name,-c0} ║ {company.Industry,c1} ║ {Math.Round(company.SharePrice, 2),c2:C} ║ {company.NumberOfShares,c3} ║");
        }
        companiesTable.AppendLine($"╚═{new('═', c0)}═╩═{new('═', c1)}═╩═{new('═', c2)}═╩═{new('═', c3)}═╝");

        return companiesTable;
    }
}