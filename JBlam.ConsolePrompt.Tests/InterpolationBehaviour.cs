namespace JBlam.ConsolePrompt.Tests;


public class InterpolationBehaviour
{
    // Note that we can't use [Theory] because the input data is bound from
    // object[], and you can't box a ref struct to an object.

    static void AssertRangeEquivalent(Range expected, Range actual, int length)
    {
        Assert.Equal(expected.GetOffsetAndLength(length), actual.GetOffsetAndLength(length));
    }
    static void AssertStringValuesEqual(
        string expected, 
        ConsolePromptInterpolationHandler noColour,
        ConsolePromptInterpolationHandler withColour)
    {
        Assert.Equal(expected, noColour.GetFormatted());
        Assert.Equal(expected, withColour.GetFormatted());
        Assert.Empty(noColour.Colours);
        var (range, colour) = Assert.Single(withColour.Colours);
        AssertRangeEquivalent(1..^1, range, expected.Length);
    }

    const int Value = 15;
    
    [Fact]
    public void Default() => AssertStringValuesEqual(
        $"[{Value}]",
        $"[{Value}]",
        $"[{Value:Green}]");

    [Fact]
    public void Hex() => AssertStringValuesEqual(
        $"[{Value:X2}]",
        $"[{Value:X2}]",
        $"[{Value:Green,X2}]");

    [Fact]
    public void GroupSeperator() => AssertStringValuesEqual(
        $"[{Value:##,#}]",
        $"[{Value:##,#}]",
        $"[{Value:Green,##,#}]");

    [Fact]
    public void Spacing() => AssertStringValuesEqual(
        $"[{Value,8}]",
        $"[{Value,8}]",
        $"[{Value,8:Green}]");

    [Fact]
    public void SpacingFormat() => AssertStringValuesEqual(
        $"[{Value,8:X2}]",
        $"[{Value,8:X2}]",
        $"[{Value,8:Green,X2}]");
}
