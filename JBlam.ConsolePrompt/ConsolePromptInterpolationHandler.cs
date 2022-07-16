using System.Runtime.CompilerServices;
using System.Text;

namespace JBlam.ConsolePrompt;

[InterpolatedStringHandler]
public ref struct ConsolePromptInterpolationHandler
{
    // Implementation notes:
    // - every time we format a thing, we strip off any colour info and then
    //   delegate the "underlying" format to a base implementation.
    // - the formatted text is stored in a buffer
    // - we store the interpreted colour against the range of chars written
    //   to the internal buffer
    // - consumers of this type will be able to get the buffer and the coloured
    //   ranges
    // We need the internal buffer mainly so that we can measure how many chars
    // get emitted when the underlying formatter prints stuff. This is why we
    // need to use the StringBuilder handler impl: the DefaultInterpolatedStringHandler
    // implementation doesn't provide that information.

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