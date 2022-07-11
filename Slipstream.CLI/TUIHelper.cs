using Slipstream.Domain.ValueObjects;

namespace Slipstream.CLI;

internal class TUIHelper
{
    internal TUIHelper PrintHeading(string text)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(new string('-', text.Length + 2));
        Console.WriteLine($" {text} ");
        Console.WriteLine(new string('-', text.Length + 2));
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
        Console.WriteLine();
        return this;
    }

    internal TUIHelper PrintStrong(string v)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(v);
        Reset();
        return this;
    }

    internal TUIHelper Print(string v)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(v);
        return this;
    }

    internal TUIHelper Error(string v)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(v);
        Reset();
        return this;
    }

    internal string Prompt(string v)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(v + ": ");
        Console.ForegroundColor = ConsoleColor.Gray;
        return Console.ReadLine() ?? "";
    }

    internal string Prompt(string prompt, string? value, string? formHelp)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"{prompt}: ");

        if (formHelp is not null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("  HELP: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(formHelp);
        }

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
}
