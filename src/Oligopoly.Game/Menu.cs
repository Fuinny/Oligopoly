namespace Oligopoly.Game;

public class Menu(string prompt, string[] options, bool canBePaused = false, string[] descriptions = default)
{
    private readonly string _prompt = prompt;
    private readonly string[] _options = options;
    private readonly string[] _descriptions = descriptions;
    private readonly bool _canBePaused = canBePaused;
    private int _selectedOption;

    /// <summary>Displays and updates menu with given prompt and options.</summary>
    /// <param name="selectionKey">The <see cref="ConsoleKey"/> that finalizes the selection of an option. Defaults to <see cref="ConsoleKey.Enter".</param>
    /// <returns>An <see cref="System.Int32"/> value, that represents selected option index.</returns>
    /// <remarks>
    /// If the menu can be paused, pressing the 'P' key will return -1.
    /// </remarks>
    public int RunMenu(ConsoleKey selectionKey = ConsoleKey.Enter)
    {
        ConsoleKey keyPressed;

        do
        {
            Console.Clear();
            Console.WriteLine(_prompt);

            for (int i = 0; i < _options.Length; i++)
            {
                if (_selectedOption == i)
                {
                    (Console.ForegroundColor, Console.BackgroundColor) = (Console.BackgroundColor, Console.ForegroundColor);
                    Console.WriteLine($"[*] {_options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"[ ] {_options[i]}");
                }
            }

            if (_descriptions != null && _descriptions.Length <= _options.Length)
            {
                Console.WriteLine("\nDescription:");
                Console.WriteLine($"{_descriptions[_selectedOption]}");
            }

            ConsoleKeyInfo keyPressedInfo = Console.ReadKey(true);
            keyPressed = keyPressedInfo.Key;

            switch (keyPressed)
            {
                case ConsoleKey.UpArrow:
                    _selectedOption--;
                    if (_selectedOption == -1)
                    {
                        _selectedOption = _options.Length - 1;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    _selectedOption++;
                    if (_selectedOption == _options.Length)
                    {
                        _selectedOption = 0;
                    }
                    break;
                case ConsoleKey.P when _canBePaused:
                    return -1;
            }
        } while (keyPressed != selectionKey);

        return _selectedOption;
    }

    /// <summary>Displays and updates menu for buying or selling shares.</summary>
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
            Console.WriteLine(_prompt);

            transactionValue = 0.0M;

            for (int i = 0; i < numberOfSharesToProcess.Length; i++)
            {
                transactionValue += numberOfSharesToProcess[i] * companies[i].SharePrice;
            }

            for (int i = 0; i < _options.Length; i++)
            {
                if (i == _selectedOption)
                {
                    (Console.ForegroundColor, Console.BackgroundColor) = (Console.BackgroundColor, Console.ForegroundColor);
                    Console.WriteLine($"[*] <{numberOfSharesToProcess[i]}{(isTransactionTypeBuying ? "" : $"/{companies[i].NumberOfShares}")}> {_options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"[ ] <{numberOfSharesToProcess[i]}> {_options[i]}");
                }
            }

            Console.WriteLine($"\nTransaction {(isTransactionTypeBuying ? "cost" : "payout")}: {Math.Round(transactionValue, 2):C}");

            ConsoleKeyInfo keyPressedInfo = Console.ReadKey(true);
            keyPressed = keyPressedInfo.Key;

            switch (keyPressed)
            {
                case ConsoleKey.UpArrow:
                    _selectedOption--;
                    if (_selectedOption == -1)
                    {
                        _selectedOption = _options.Length - 1;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    _selectedOption++;
                    if (_selectedOption == _options.Length)
                    {
                        _selectedOption = 0;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (numberOfSharesToProcess[_selectedOption] > 0)
                    {
                        numberOfSharesToProcess[_selectedOption]--;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (isTransactionTypeBuying)
                    {
                        if (transactionValue + companies[_selectedOption].SharePrice <= money)
                        {
                            numberOfSharesToProcess[_selectedOption]++;
                        }
                    }
                    else
                    {
                        if (numberOfSharesToProcess[_selectedOption] < companies[_selectedOption].NumberOfShares)
                        {
                            numberOfSharesToProcess[_selectedOption]++;
                        }
                    }
                    break;
            }
        } while (keyPressed != ConsoleKey.Enter);

        Console.Clear();
        Console.WriteLine(_prompt);
        Console.WriteLine("\nTransaction completed.");
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(true);
    }

    /// <summary>Displays and updates menu for setting up the initial amount of money.</summary>
    /// <returns>A <see cref="System.Decimal"/> value that represents selected amount of money.</returns>
    public decimal RunMoneySetupMenu()
    {
        const decimal MinCustomMoney = 3000M, MaxCustomMoney = 45000M;

        decimal customMoney = MinCustomMoney;

        ConsoleKey keyPressed;

        while (true)
        {
            Console.Clear();
            Console.WriteLine(_prompt);
            Console.WriteLine($"\nYour starting amount of money will be set to {customMoney:C}");
            Console.WriteLine("Use up and down arrow keys and enter to select an option:\n");

            for (int i = 0; i < _options.Length; i++)
            {
                if (i == _selectedOption)
                {
                    (Console.ForegroundColor, Console.BackgroundColor) = (Console.BackgroundColor, Console.ForegroundColor);
                    Console.WriteLine($"[*] {_options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"[ ] {_options[i]}");
                }
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            keyPressed = keyInfo.Key;

            switch (keyPressed)
            {
                case ConsoleKey.UpArrow:
                    _selectedOption--;
                    if (_selectedOption == -1)
                    {
                        _selectedOption = _options.Length - 1;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    _selectedOption++;
                    if (_selectedOption > _options.Length - 1)
                    {
                        _selectedOption = 0;
                    }
                    break;
                case ConsoleKey.Enter:
                    if (_selectedOption == 0)
                    {
                        if (customMoney + 100 <= MaxCustomMoney)
                        {
                            customMoney += 100;
                        }
                    }
                    else if (_selectedOption == 1)
                    {
                        if (customMoney - 100 >= MinCustomMoney)
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