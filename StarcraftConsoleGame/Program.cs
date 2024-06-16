namespace StarcraftConsoleGame;

internal static class Program
{
    private static void Main()
    {
        Console.Title = "StarCraft Console Game";
        Console.SetWindowSize(640, 800);
        Writer.WriteFile("StartupText.txt");
        Console.WriteLine("\n");
        Writer.SlowWrite("Welcome to StarCraft:The forgotten prophecy!", 50);

        while (true)
        {
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. New Game, 2. Exit");
            var choice = Console.ReadKey(true);
            switch (choice)
            {
                case { Key: ConsoleKey.D1 }:
                    GameManager.NewGame();
                    return;
                case { Key: ConsoleKey.D2 }:
                    Environment.Exit(0);
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
        //EventManager.NewGame();
    }
}