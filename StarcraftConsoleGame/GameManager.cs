using StarcraftConsoleGame.Enemies;

namespace StarcraftConsoleGame;

public static class GameManager
{
    
    // ReSharper disable once InconsistentNaming
    public static readonly Player ActivePlayer = new Zeratul();
    public static readonly Map ActiveMap = new Map(5, "Aiur");
    
    
    public static void NewGame()
    {
        Console.Clear();
        //Writer.SlowWriteFile("NewGame.txt");
        // Console.WriteLine("Would you like to play through the fighting tutorial? (Y/N)");
        // var choice = Console.ReadKey(true);
        // if (choice.Key is ConsoleKey.Y)
        // {
        //     Tutorial();
        // }
        // else
        // {
        //     StartMainLoop();
        // }
        StartMainLoop();
    }

    // ReSharper disable once UnusedMember.Local
    private static void Tutorial()
    {
        //Writer.SlowWriteFile("Tutorial.txt");
        //Console.WriteLine("Press any key to continue...");
        //Console.ReadKey();
        List<Entity> tutorialEnemies = [new Zergling(), new Zergling(), new Zergling()];
        BattleManager.Encounter(tutorialEnemies);
        StartMainLoop();
    }

    public static void GameOver()
    {
        //Writer.WriteFile("GameOver.txt");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);
        //StatManager.DisplayStats();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(true);
        Environment.Exit(0);

    }

    private static void StartMainLoop()
    {
        DisplayMenu();
        MainLoop();
    }
    
    private static void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine("What would you like to do:");
        Console.WriteLine("1. Go further, 2. Stats and upgrades, 3. Exit");
    }
    
    private static void MainLoop()
    {
        while (true)
        {
            var choice = Console.ReadKey(true);
            Console.Clear();
            Console.WriteLine("What would you like to do:");
            Console.WriteLine("1. Go further, 2. Stats and upgrades, 3. Exit");
            switch (choice)
            {
                case { Key: ConsoleKey.D1 }:
                    ActivePlayer.MoveUp();
                    break;
                case { Key: ConsoleKey.D2 }:
                    ActivePlayer.DisplayStats();
                    break;
                case { Key: ConsoleKey.D3 }:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
    
    public static void Victory()
    {
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Writer.SlowWrite("Congratulations! You have defeated the Zerg and reached your starship!", 75, ConsoleColor.Yellow);
        // Console.WriteLine("Press any key to continue...");
        // Console.ReadKey(true);
        //StatManager.DisplayStats();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(true);
        Environment.Exit(0);
    }
}