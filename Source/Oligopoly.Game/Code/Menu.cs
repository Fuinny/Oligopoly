namespace Oligopoly.Game;

public class Menu(string prompt, string[] options, bool canBePaused = false, string[] descriptions = default)
{
    private readonly string Prompt = prompt;
    private readonly string[] Options = options;
    private readonly string[] Descriptions = descriptions;
    private readonly bool CanBePaused = canBePaused;
    private int SelectedOption;

    /// <summary>Displays and updates general menu with given prompt and options.</summary>
    /// <returns>An <see cref="System.Int32"/> value, that represents selected option index.</returns>
    public int RunMenu()
    {
        ConsoleKey keyPressed;

        do
        {
            Console.Clear();
            Console.WriteLine(Prompt);

            for (int i = 0; i < Options.Length; i++)
            {
                if (SelectedOption == i)
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

            if (Descriptions != null && Descriptions.Length <= Options.Length)
            {
                Console.WriteLine("\nDescription:");
                Console.WriteLine($"{Descriptions[SelectedOption]}");
            }

            ConsoleKeyInfo keyPressedInfo = Console.ReadKey(true);
            keyPressed = keyPressedInfo.Key;

            switch (keyPressed)
            {
                case ConsoleKey.UpArrow:
                    SelectedOption--;
                    if (SelectedOption == -1)
                    {
                        SelectedOption = Options.Length - 1;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    SelectedOption++;
                    if (SelectedOption == Options.Length)
                    {
                        SelectedOption = 0;
                    }
                    break;
                case ConsoleKey.P when CanBePaused:
                    return -1;
            }
        } while (keyPressed != ConsoleKey.Enter);

        return SelectedOption;
    }

    /// <summary>Displays and updates menu for buying of selling shares.</summary>
    /// <param name="numberOfSharesToProcess">Number of shares to buy or sell for each company.</param>
    /// <param name="transactionValue">The total value of the current transaction.</param>
    /// <param name="money">The player's current amount of money.</param>
    /// <param name="companies">List of all in-game companies.</param>
    /// <param name="isTransactionTypeBuying">Flag indicating whether the transaction is buying or selling.</param>
    public void RunTransactionMenu(ref int[] numberOfSharesToProcess, ref decimal transactionValue, decimal money, List<Company> companies, bool isTransactionTypeBuying)
    {
        ConsoleKey keyPressed;

        do
        {
            Console.Clear();
            Console.WriteLine(Prompt);

            for (int i = 0; i < numberOfSharesToProcess.Length; i++)
            {
                transactionValue += numberOfSharesToProcess[i] * companies[i].SharePrice;
            }

            for (int i = 0; i < Options.Length; i++)
            {
                if (i == SelectedOption)
                {
                    (Console.ForegroundColor, Console.BackgroundColor) = (Console.BackgroundColor, Console.ForegroundColor);
                    Console.WriteLine($"[*] <{numberOfSharesToProcess[i]}{(isTransactionTypeBuying ? "" : $"/{companies[i].NumberOfShares}")}> {Options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"[ ] <{numberOfSharesToProcess[i]}> {Options[i]}");
                }
            }

            Console.WriteLine($"\nTransaction {(isTransactionTypeBuying ? "cost" : "payout")}: {Math.Round(transactionValue, 2):C}");

            if (Descriptions != null && Descriptions.Length <= Options.Length)
            {
                Console.WriteLine("\nDescription:");
                Console.WriteLine($"{Descriptions[SelectedOption]}");
            }

            ConsoleKeyInfo keyPressedInfo = Console.ReadKey(true);
            keyPressed = keyPressedInfo.Key;

            switch (keyPressed)
            {
                case ConsoleKey.UpArrow:
                    SelectedOption--;
                    if (SelectedOption == -1)
                    {
                        SelectedOption = Options.Length - 1;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    SelectedOption++;
                    if (SelectedOption == Options.Length)
                    {
                        SelectedOption = 0;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (numberOfSharesToProcess[SelectedOption] > 0)
                    {
                        numberOfSharesToProcess[SelectedOption]--;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (isTransactionTypeBuying)
                    {
                        if (transactionValue + companies[SelectedOption].SharePrice <= money)
                        {
                            numberOfSharesToProcess[SelectedOption]++;
                        }
                    }
                    else
                    {
                        if (numberOfSharesToProcess[SelectedOption] < companies[SelectedOption].NumberOfShares)
                        {
                            numberOfSharesToProcess[SelectedOption]++;
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

    /// <summary>Displays and updates a menu for setting up the initial amount of money.</summary>
    /// <returns>A <see cref="System.Decimal"/> value that represents selected amount of money.</returns>
    public decimal RunMoneySetupMenu()
    {
        const decimal standardCustomMoney = 1000M;
        decimal customMoney = standardCustomMoney;
        ConsoleKey keyPressed;

        while (true)
        {
            Console.Clear();
            Console.WriteLine(Prompt);
            Console.WriteLine($"\nYour starting amount of money will be set to {customMoney:C}");
            Console.WriteLine("Use up and down arrow keys and enter to select an option:\n");

            for (int i = 0; i < Options.Length; i++)
            {
                if (i == SelectedOption)
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

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            keyPressed = keyInfo.Key;

            switch (keyPressed)
            {
                case ConsoleKey.UpArrow:
                    SelectedOption--;
                    if (SelectedOption == -1)
                    {
                        SelectedOption = Options.Length - 1;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    SelectedOption++;
                    if (SelectedOption > Options.Length - 1)
                    {
                        SelectedOption = 0;
                    }
                    break;
                case ConsoleKey.Enter:
                    if (SelectedOption == 0)
                    {
                        if (customMoney + 100 <= 45000)
                        {
                            customMoney += 100;
                        }
                    }
                    else if (SelectedOption == 1)
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

    /// <summary>Draws table with information about all in-game companies to the console.</summary>
    /// <param name="companies">List of all in-game companies.</param>
    /// <returns>A <see cref="System.Text.StringBuilder"/> object, that contains formatted table.</returns>
    public static StringBuilder DrawCompaniesTable(List<Company> companies)
    {
        const int CompanyColumnWidth = 30, IndustryColumnWidth = 10, ShareColumnWidth = 20, YouHaveColumnWidth = 15;

        StringBuilder companiesTable = new();
        companiesTable.AppendLine($"╔═{new('═', CompanyColumnWidth)}═╦═{new('═', IndustryColumnWidth)}═╦═{new('═', ShareColumnWidth)}═╦═{new('═', YouHaveColumnWidth)}═╗");
        companiesTable.AppendLine($"║ {"Company",-CompanyColumnWidth} ║ {"Industry",IndustryColumnWidth} ║ {"Share Price",ShareColumnWidth} ║ {"You Have",YouHaveColumnWidth} ║");
        companiesTable.AppendLine($"╠═{new('═', CompanyColumnWidth)}═╬═{new('═', IndustryColumnWidth)}═╬═{new('═', ShareColumnWidth)}═╬═{new('═', YouHaveColumnWidth)}═╣");
        foreach (Company company in companies)
        {
            companiesTable.AppendLine($"║ {company.Name,-CompanyColumnWidth} ║ {company.Industry,IndustryColumnWidth} ║ {Math.Round(company.SharePrice, 2),ShareColumnWidth:C} ║ {company.NumberOfShares,YouHaveColumnWidth} ║");
        }
        companiesTable.AppendLine($"╚═{new('═', CompanyColumnWidth)}═╩═{new('═', IndustryColumnWidth)}═╩═{new('═', ShareColumnWidth)}═╩═{new('═', YouHaveColumnWidth)}═╝");

        return companiesTable;
    }
}