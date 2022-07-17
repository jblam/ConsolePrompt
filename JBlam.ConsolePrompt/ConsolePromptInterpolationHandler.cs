using System.Runtime.CompilerServices;
using System.Text;

namespace JBlam.ConsolePrompt;

/// <summary>
/// Provides a handler used by the language compiler to append interpolated 
/// strings into <see cref="Out.Write(ConsolePromptInterpolationHandler)"/>
/// and <see cref="Out.WriteLine(ConsolePromptInterpolationHandler)"/>.
/// </summary>
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

    /// <summary>
    /// Creates a handler used to write an interpolated string to the console.
    /// </summary>
    /// <param name="literalLength">
    /// The number of literal characters outside of interpolation expressions 
    /// in the interpolated string.
    /// </param>
    /// <param name="formattedCount">
    /// The number of interpolation expressions in the interpolated string.
    /// </param>
    public ConsolePromptInterpolationHandler(int literalLength, int formattedCount)
    {
        // JB 2022-07-17: following the reference source implementation of StringBuilder's
        // handler, we're using the estimated "11 chars per formatted item".
        b = new(literalLength + formattedCount * 11);
        h = new(literalLength, formattedCount, b);
        colours = new(formattedCount);
    }

    /// <summary>Writes the specified string to the handler.</summary>
    /// <param name="value">The string to write.</param>
    public void AppendLiteral(string value)
    {
        h.AppendLiteral(value);
    }

    /// <summary>Writes the specified value to the handler.</summary>
    /// <param name="value">The value to write.</param>
    public void AppendFormatted<T>(T value)
    {
        AppendFormattedImpl(value);
    }

    /// <summary>Writes the specified value to the handler.</summary>
    /// <param name="value">The value to write.</param>
    /// <param name="alignment">
    /// The minimum number of characters that should be written for this value.
    /// If the value is negative, it indicates left-aligned and the required minimum is the absolute value.
    /// </param>
    public void AppendFormatted<T>(T value, int alignment)
    {
        AppendFormattedImpl(value, alignment);
    }

    /// <summary>Writes the specified value to the handler.</summary>
    /// <param name="value">The value to write.</param>
    /// <param name="format">The format string.</param>
    public void AppendFormatted<T>(T value, string? format)
    {
        if (ColourFormat.TryParse(format, out var colour, out var rest))
        {
            var range = AppendFormattedImpl(value, rest.ToString());
            colours.Add((range, colour));
        }
        else
        {
            AppendFormattedImpl(value, format);
        }
    }
    /// <summary>Writes the specified value to the handler.</summary>
    /// <param name="value">The value to write.</param>
    /// <param name="format">The format string.</param>
    /// <param name="alignment">
    /// The minimum number of characters that should be written for this value.
    /// If the value is negative, it indicates left-aligned and the required minimum is the absolute value.
    /// </param>
    public void AppendFormatted<T>(T value, int alignment, string? format)
    {
        if (ColourFormat.TryParse(format, out var colour, out var rest))
        {
            var range = AppendFormattedImpl(value, alignment, rest.ToString());
            colours.Add((range, colour));
        }
        else
        {
            AppendFormattedImpl(value, alignment, format);
        }
    }
    Range AppendFormattedImpl<T>(T value, string? format)
    {
        var previousLength = b.Length;
        h.AppendFormatted(value, format);
        return previousLength..b.Length;
    }
    Range AppendFormattedImpl<T>(T value)
    {
        var previousLength = b.Length;
        h.AppendFormatted(value);
        return previousLength..b.Length;
    }
    Range AppendFormattedImpl<T>(T value, int alignment)
    {
        var previousLength = b.Length;
        h.AppendFormatted(value, alignment);
        return previousLength..b.Length;
    }
    Range AppendFormattedImpl<T>(T t, int alignment, string? format)
    {
        var previousLength = b.Length;
        h.AppendFormatted(t, alignment, format);
        return previousLength..b.Length;
    }

    /// <summary>
    /// Gets a list of the ranges of the interpolated string for which a
    /// <see cref="ConsoleColor"/> value was specified.
    /// </summary>
    public IReadOnlyList<(Range, ConsoleColor)> Colours => colours;
    /// <summary>
    /// Gets the value of the formatted string.
    /// </summary>
    public string GetFormatted() => b.ToString();
}