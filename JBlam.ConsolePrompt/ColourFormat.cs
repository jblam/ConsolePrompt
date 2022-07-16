namespace JBlam.ConsolePrompt;

internal static class ColourFormat
{
    public static bool TryParse(string? format, out ConsoleColor colour, out ReadOnlySpan<char> rest)
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
            && Enum.TryParse<ConsoleColor>(span[..partition], out colour)
            && Enum.IsDefined(colour))
        {
            rest = span[(partition + 1)..];
            return true;
        }
        else if (Enum.TryParse<ConsoleColor>(format, out colour)
            && Enum.IsDefined(colour))
        {
            rest = default;
            return true;
        }
        colour = default;
        rest = format.AsSpan();
        return false;
    }
}