namespace JBlam.ConsolePrompt;

/// <summary>
/// Provides equivalent methods to <see cref="Console.Write(string?)"/>
/// and <see cref="Console.WriteLine(string)"/> which support colour in
/// the formatting part of an interpolated string.
/// </summary>
public static class Out
{
    /// <summary>
    /// Writes the specified interpolated string, followed by the current line terminator,
    /// to the standard output stream.
    /// </summary>
    /// <param name="handler">The interpolated string to write.</param>
    /// <remarks>
    /// Colours are specified by the string name of a member of <see cref="ConsoleColor"/>;
    /// the colour may be given alone (<c>Out.WriteLine($"{value:Green}")</c>),
    /// or as the prefix to another format string (<c>Out.WriteLine("$Value: {value:Green,X2}");</c>).
    /// </remarks>
    public static void WriteLine(ConsolePromptInterpolationHandler handler) 
    {
        Out.Write(handler);
        Console.WriteLine();
    }
    /// <summary>
    /// Writes the specified interpolated string to the standard output stream.
    /// </summary>
    /// <param name="handler">The interpolated string to write.</param>
    /// <remarks>
    /// Colours are specified by the string name of a member of <see cref="ConsoleColor"/>;
    /// the colour may be given alone (<c>Out.Write($"{value:Green}")</c>),
    /// or as the prefix to another format string (<c>Out.Write("$Value: {value:Green,X2}");</c>).
    /// </remarks>
    public static void Write(ConsolePromptInterpolationHandler handler)
    {
        var result = handler.GetFormatted();
        Index position = 0;
        foreach (var (range, colour) in handler.Colours)
        {
            Console.Write(result[position..range.Start]);
            Console.ForegroundColor = colour;
            Console.Write(result[range]);
            Console.ResetColor();
            position = range.End;
        }
        Console.Write(result[position..]);
    }
}
