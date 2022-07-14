namespace JBlam.ConsolePrompt;
public static class Out
{
    public static void WriteLine(ConsolePromptInterpolationHandler handler) 
    {
        Out.Write(handler);
        Console.WriteLine();
    }
    public static void Write(ConsolePromptInterpolationHandler handler)
    {
        // JB 2022-07-02 perf considerations:
        // - Console.Write does not take a (ReadOnly)Span<char>
        // - it _does_ take an old-skool (char[], int, int)
        // Also worth considering allocs from the stringbuilder;
        // we could enumerate chunks instead.
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
