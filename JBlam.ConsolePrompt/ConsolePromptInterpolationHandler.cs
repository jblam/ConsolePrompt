using System.Runtime.CompilerServices;
using System.Text;

namespace JBlam.ConsolePrompt;

[InterpolatedStringHandler]
public ref struct ConsolePromptInterpolationHandler
{
    readonly StringBuilder.AppendInterpolatedStringHandler h;
    readonly StringBuilder b;
    readonly List<(Range, ConsoleColor)> colours;

    public ConsolePromptInterpolationHandler(int literalLength, int formattedCount)
    {
        // JB 2022-07-02: literalLength is the _minimum_ length; if formattedCount > 0
        // we should probably request more?
        b = new(literalLength);
        h = new(literalLength, formattedCount, b);
        colours = new(formattedCount);
    }
    public void AppendLiteral(string s)
    {
        h.AppendLiteral(s);
    }

    public void AppendFormatted<T>(T t)
    {
        AppendFormattedImpl(t);
    }
    public void AppendFormatted<T>(T t, int alignment)
    {
        AppendFormattedImpl(t, alignment);
    }

    public void AppendFormatted<T>(T t, string? format)
    {
        if (ColourFormat.TryParse(format, out var colour, out var rest))
        {
            var range = AppendFormattedImpl(t, rest.ToString());
            colours.Add((range, colour));
        }
        else
        {
            AppendFormattedImpl(t, format);
        }
    }
    public void AppendFormatted<T>(T t, int alignment, string? format)
    {
        if (ColourFormat.TryParse(format, out var colour, out var rest))
        {
            var range = AppendFormattedImpl(t, alignment, rest.ToString());
            colours.Add((range, colour));
        }
        else
        {
            AppendFormattedImpl(t, alignment, format);
        }
    }
    Range AppendFormattedImpl<T>(T t, string? format)
    {
        var previousLength = b.Length;
        h.AppendFormatted(t, format);
        return previousLength..b.Length;
    }
    Range AppendFormattedImpl<T>(T t)
    {
        var previousLength = b.Length;
        h.AppendFormatted(t);
        return previousLength..b.Length;
    }
    Range AppendFormattedImpl<T>(T t, int alignment)
    {
        var previousLength = b.Length;
        h.AppendFormatted(t, alignment);
        return previousLength..b.Length;
    }
    Range AppendFormattedImpl<T>(T t, int alignment, string? format)
    {
        var previousLength = b.Length;
        h.AppendFormatted(t, alignment, format);
        return previousLength..b.Length;
    }

    public IReadOnlyList<(Range, ConsoleColor)> Colours => colours;
    public string GetFormatted() => b.ToString();
}