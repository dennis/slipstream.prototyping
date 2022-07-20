namespace Slipstream.CLI;

internal class TUIHelper
{
    private readonly List<string> _prefixes = new();

    public TUIHelper()
    {
    }

    public TUIHelper(List<string> prefixes)
    {
        _prefixes = prefixes;
    }

    internal TUIHelper NewScope(string prefix)
    {
        var newPrefixes = new List<string>();
        newPrefixes.AddRange(_prefixes);
        newPrefixes.Add(prefix);
        return new TUIHelper(newPrefixes);
    }

    internal TUIHelper PrintHeading(string text)
    {
        RenderPrefix();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(new string('-', text.Length + 2));

        RenderPrefix();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($" {text} ");

        RenderPrefix();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(new string('-', text.Length + 2));

        Reset();

        return this;
    }

    internal TUIHelper Debug(string v)
    {
        RenderPrefix();
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(v);
        Reset();

        return this;
    }

    internal TUIHelper Reset()
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        return this;
    }

    internal TUIHelper Spacer()
    {
        RenderPrefix();
        Console.WriteLine();
        return this;
    }

    internal TUIHelper PrintStrong(string v)
    {
        RenderPrefix();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(v);
        Reset();
        return this;
    }

    internal TUIHelper Print(string v)
    {
        RenderPrefix();
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(v);
        return this;
    }

    internal TUIHelper Error(string v)
    {
        RenderPrefix();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(v);
        Reset();
        return this;
    }

    internal string Prompt(string v)
    {
        RenderPrefix();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(v + ": ");
        Console.ForegroundColor = ConsoleColor.Gray;
        return Console.ReadLine() ?? "";
    }

    internal string Prompt(string prompt, string? value, string? formHelp)
    {
        RenderPrefix();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"{prompt}: ");

        if (formHelp is not null)
        {
            RenderPrefix();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("  HELP: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(formHelp);
        }

        RenderPrefix();

        Console.ForegroundColor = ConsoleColor.Cyan;

        if (value != null)
        {
            Console.Write($"[current: '{value}']: ");
        }
        else
        {
            Console.Write($": ");
        }
        Console.ForegroundColor = ConsoleColor.White;

        return Console.ReadLine() ?? "";
    }

    internal char ReadKey()
    {
        var c = Console.ReadKey().KeyChar;
        Spacer();
        Spacer();
        return c;
    }

    private void RenderPrefix()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(string.Join('/', _prefixes) + ">> ");
    }
}
