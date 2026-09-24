namespace VanguardCore.Helpers;

public static class ConsoleHelper
{
    public static void DrawText(string text, Color color = Color.White, bool newLine = true)
    {
        Console.ForegroundColor = color switch
        {
            Color.White => ConsoleColor.White,
            Color.Gray => ConsoleColor.Gray,
            Color.Yellow => ConsoleColor.Yellow,
            Color.Red => ConsoleColor.Red,
            Color.Green => ConsoleColor.Green,
            _ => ConsoleColor.White
        };

        Console.Write(text);

        if (newLine) Console.Write(Environment.NewLine);
        
        Console.ResetColor();
    }
    public static void DrawError(string text, Color color = Color.White, bool newLine = true)
    {
        Console.ForegroundColor = color switch
        {
            Color.White => ConsoleColor.White,
            Color.Gray => ConsoleColor.Gray,
            Color.Yellow => ConsoleColor.Yellow,
            Color.Red => ConsoleColor.Red,
            Color.Green => ConsoleColor.Green,
            _ => ConsoleColor.White
        };

        Console.Error.Write(text);

        if (newLine) Console.Error.Write(Environment.NewLine);

        Console.ResetColor();
        Environment.Exit(1);
    }
    public static string? TakeInput(string prefix = " > ", Color prefixColor = Color.Green, Color textColor = Color.White)
    {
        Console.ForegroundColor = prefixColor switch
        {
            Color.White => ConsoleColor.White,
            Color.Gray => ConsoleColor.Gray,
            Color.Yellow => ConsoleColor.Yellow,
            Color.Red => ConsoleColor.Red,
            Color.Green => ConsoleColor.Green,
            _ => ConsoleColor.White
        };
        DrawText(prefix, prefixColor, false);
        Console.ForegroundColor = textColor switch
        {
            Color.White => ConsoleColor.White,
            Color.Gray => ConsoleColor.Gray,
            Color.Yellow => ConsoleColor.Yellow,
            Color.Red => ConsoleColor.Red,
            Color.Green => ConsoleColor.Green,
            _ => ConsoleColor.White
        };
        string? result = Console.ReadLine();
        Console.ResetColor();
        return result;
    }
}

public enum Color
{
    White,
    Gray,
    Yellow,
    Red,
    Green
}