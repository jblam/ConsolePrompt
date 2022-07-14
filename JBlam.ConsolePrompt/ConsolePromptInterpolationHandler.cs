using System.Runtime.CompilerServices;
using System.Text;

namespace JBlam.ConsolePrompt;

[InterpolatedStringHandler]
public ref struct ConsolePromptInterpolationHandler
{
    readonly StringBuilder b;
    readonly List<(Range, ConsoleColor)> colours;

    public ConsolePromptInterpolationHandler(int literalLength, int formattedCount)
    {
        // JB 2022-07-02: literalLength is the _minimum_ length; if formattedCount > 0
        // we should probably request more?
        b = new(literalLength);
        colours = new(formattedCount);
    }
    public void AppendLiteral(string s)
    {
        b.Append(s);
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
        // TODO: parse off any colour specifier on the front of format
        if (TryParseColourFormat(format, out var colour, out var rest))
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
        // TODO: parse off any colour specifier on the front of format
        if (TryParseColourFormat(format, out var colour, out var rest))
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
        StringBuilder.AppendInterpolatedStringHandler h = new(0, 1, b);
        h.AppendFormatted(t, format);
        return previousLength..b.Length;
    }
    Range AppendFormattedImpl<T>(T t)
    {
        var previousLength = b.Length;
        StringBuilder.AppendInterpolatedStringHandler h = new(0, 1, b);
        h.AppendFormatted(t);
        return previousLength..b.Length;
    }
    Range AppendFormattedImpl<T>(T t, int alignment)
    {
        var previousLength = b.Length;
        StringBuilder.AppendInterpolatedStringHandler h = new(0, 1, b);
        h.AppendFormatted(t, alignment);
        return previousLength..b.Length;
    }
    Range AppendFormattedImpl<T>(T t, int alignment, string? format)
    {
        var previousLength = b.Length;
        StringBuilder.AppendInterpolatedStringHandler h = new(0, 1, b);
        h.AppendFormatted(t, alignment, format);
        return previousLength..b.Length;
    }
    bool TryParseColourFormat(string? format, out ConsoleColor colour, out ReadOnlySpan<char> rest)
    {
        if (format is null)
        {
            colour = default;
            rest = default;
            return false;
        }
        var span = format.AsSpan();
        if (span.IndexOf(',') is var partition
            && partition >= 0
            // JB 2022-07-14: we can be confident that if a format substring is a legal colour,
            // and the user is passing it to our colourful handler, it's intended to be a colour.
            // However, if we don't parse a colour, we can't emit an error because commas are legal
            // in other kinds of format string too.
            // We might consider having a more-unusual separator char?
            && Enum.TryParse<ConsoleColor>(span[..partition], out colour))
        {
            rest = span[(partition + 1)..];
            return true;
        }
        else if (Enum.TryParse<ConsoleColor>(format, out colour))
        {
            rest = default;
            return true;
        }
        colour = default;
        rest = format.AsSpan();
        return false;
    }

    public IReadOnlyList<(Range, ConsoleColor)> Colours => colours;
    public string GetFormatted() => b.ToString();
}