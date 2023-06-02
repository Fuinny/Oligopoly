# Oligopoly
Oligopoly is a command-line interface (CLI) game written in C# that allows players to buy and sell shares of various in-game companies. 
The value of these shares is affected by in-game events that occur every turn.
The goal of the player is to buy shares of the company and then try to sell them at a higher price in order to increase their net worth.

```
╔════════════════════════════════╦════════════╦══════════════════════╦═════════════════╗
║ Company                        ║   Industry ║          Share Price ║        You Have ║
╠════════════════════════════════╬════════════╬══════════════════════╬═════════════════╣
║ Bingoo                         ║        Web ║              1372.28 ║               1 ║
║ Quantum Software               ║   Software ║              2094.98 ║               0 ║
║ Edison Incorporated            ║     Energy ║              1036.80 ║               2 ║
║ Netfilm                        ║     Movies ║               838.80 ║               1 ║
║ COBRA Security Consulting      ║       Army ║              1283.29 ║               4 ║
║ Rockland Games                 ║      Games ║               885.15 ║               0 ║
║ Astro Technologies             ║      Space ║              2319.93 ║               0 ║
╚════════════════════════════════╩════════════╩══════════════════════╩═════════════════╝

You have: 900.00$     Your Net Worth: 10317.83$     Current Turn: 10

[*] Wait For Market Change
[ ] Buy
[ ] Sell
[ ] More About Companies
```
## Disclaimer
Please note that all in-game companies and events in Oligopoly are completely fictitious and have no relation to real entities or events. 
They do not reflect the author's point of view on these companies and events, and coincidences are accidental. The game is intended for entertainment purposes only.

This is not a professional project, so don't expect high quality code or regular updates.

## How to start the game
You can start the game using any of these methods:

<details>
  
  <summary>
    Download release version of the game
  </summary>
  
  > 1. Go to the [releases](https://github.com/Fuinny/Oligopoly/releases) page.
  > 2. Find the latest game release and download .zip file, suitable for your operating system.
  > 3. Unzip the downloaded file to the folder where you will launch the game.
  
</details>
<details>
  
  <summary>
    Compile the game using .NET Software Development Kit (SDK)
  </summary>
  
  > 1. Make sure that you have [.NET Software Development Kit (SDK)](https://dotnet.microsoft.com/en-us/download) installed.
  > 2. Download the game code.
  > 3. Open a command prompt (or terminal) and navigate to the directory where the downloaded code is located.
  > 4. Run the ```dotnet run``` command from the command line (or terminal).
  
</details>
<details>
  
  <summary>
    Play online version of the game
  </summary>
  
  > 1. Go to [this](https://github.com/dotnet/dotnet-console-games) repository.
  > 2. Find Oligopoly in the table and click ```Play Now```.
  > 3. Also try to play other games from this repository :D
  
  > **Note:** this version of the game is different from the one presented in this repository. If you want to add your companies or events, use the methods presented before.
</details>

After following any of these methods, you should be able to run Oligopoly in your command line (terminal). If for some reason you didn't succeed, let me know :D

Also, thanks for playing or trying to play this simple CLI game! It means a lot :P

## How to add your own company and event
This game supports the ability to add your companies and events to the game or edit existing ones.

To make these changes, you need to edit the .xml files in Data folder. 

You can read more information about the files structure [here](https://github.com/Fuinny/Oligopoly/blob/master/DOCUMENTATION.md).

## License
This project is licensed under the [MIT License](https://github.com/Fuinny/Oligopoly/blob/master/LICENSE.md).
