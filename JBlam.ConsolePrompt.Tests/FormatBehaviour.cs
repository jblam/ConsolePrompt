namespace JBlam.ConsolePrompt.Tests;

public class FormatBehaviour
{
    [Theory]
    // There are two branches to cover:
    // - there is at least one comma, in which case we attempt to parse the left-hand side of it;
    // - there is no comma, in which case we attempt to parse the whole string
    // Since we're using string.AsSpan, nulls are equivalent to empty strings.
    [InlineData(null, null, null)]
    // We can parse off the  colour
    [InlineData("Green", ConsoleColor.Green, "")]
    [InlineData("Green,X2", ConsoleColor.Green, "X2")]
    // When we can't, the whole format string is used.
    [InlineData("not-a-colour", null, "not-a-colour")]
    [InlineData("not-a-colour,1234", null, "not-a-colour,1234")]
    // This is unfortunate: Enum.Parse will coerce integers into the target type.
    // We might consider validating that the substring doesn't lead with a numeric char?
    [InlineData("0", default(ConsoleColor), "")]
    [InlineData("000,00", default(ConsoleColor), "00")]
    // However, we do explicitly test that the parsed value is valid, so we can reject these:
    [InlineData("999", null, "999")]
    [InlineData("999,00", null, "999,00")]
    // When there are multiple commas, we only look at the first token
    [InlineData("Green,Black,Red", ConsoleColor.Green, "Black,Red")]
    [InlineData("not-a-colour,Black,Red", null, "not-a-colour,Black,Red")]
    public void ParsesFormat(string? formatString, ConsoleColor? expected, string? expectedRest)
    {
        if (ColourFormat.TryParse(formatString, out var colour, out var rest))
        {
            Assert.Equal(expected, colour);
            Assert.Equal(expectedRest, rest.ToString());
        }
        else
        {
            Assert.Null(expected);
            Assert.Equal(expectedRest, formatString);
        }
    }
}
