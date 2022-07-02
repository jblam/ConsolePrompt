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
        b.Append(t);
    }

    public void AppendFormatted<T>(T t, string? format)
    {
        // TODO: parse off any colour specifier on the front of format
        if (t is IFormattable f)
        {
            b.Append(f.ToString(format, null));
        }
        else
        {
            b.Append(t);
        }
    }
    // TODO: implement overloads with alignment.

    public IReadOnlyList<(Range, ConsoleColor)> Colours => colours;
    public string GetFormatted() => b.ToString();
}