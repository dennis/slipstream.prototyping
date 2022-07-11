using Slipstream.Domain.Forms;

namespace Slipstream.CLI;

public class ConsoleFormVisitor : IFormCollectionVisitor
{
    public void VisitStringFormElement(StringFormElement element)
    {
        Prompt(element.Name, element.Value);

        element.Value = Console.ReadLine();
    }

    public void VisitUnsupportedFormElement(UnsupportedFormElement element)
    {
        Console.WriteLine("Found UnsupportedFormElement");
    }

    private static void Prompt(string prompt, string? value)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"  > {prompt} ");

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
    }
}