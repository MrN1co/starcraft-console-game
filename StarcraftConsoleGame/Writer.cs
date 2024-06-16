namespace StarcraftConsoleGame;

public static class Writer
{
    public static void SlowWriteFile(string filePath, int delay)
    {
        IEnumerable<string> lines;
        try
        {
            lines = File.ReadLines(filePath);
        }
        catch (IOException e)
        {
            Console.WriteLine("Error reading file: " + e.Message);
            throw;
        }
        foreach (var line in lines)
        {
            Console.WriteLine(line, delay);
        }
    }
    
    public static void WriteFile(string filePath)
    {
        IEnumerable<string> lines;
        try
        {
            lines = File.ReadLines(filePath);
        }
        catch (IOException e)
        {
            Console.WriteLine("Error reading file: " + e.Message);
            throw;
        }
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
    }
    public static void SlowWrite(string message, int delay, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        foreach (var character in message)
        {
            
            if (character is '.' or '!' or '?')
            {
                delay *= 4;
                Console.Write(character);
                Thread.Sleep(delay);
                delay /= 4;
                continue;
            }
            Console.Write(character);
            Thread.Sleep(delay);
        }
        Console.Write("\n");
        Console.ResetColor();
    }
    
    public static void WriteColor(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(message);
        Console.ResetColor();
    }
    
    public static void WriteLineColor(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    
    
}