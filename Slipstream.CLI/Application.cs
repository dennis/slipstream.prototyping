using Slipstream.Core;
using Slipstream.Core.Forms;

namespace Slipstream.CLI;

public class Application
{
    private readonly IRegistry _registry;
    private readonly IFormGenerator _formGenerator;

    public Application(IRegistry registry, IFormGenerator formGenerator)
    {
        _registry = registry;
        _formGenerator = formGenerator;
    }

    public void RunAsync()
    {
        Console.WriteLine("Slipstream");

        // This is configured via UI. We'll just create some instances
        var c = _registry.GetComponent("Dummy");

        foreach (var instanceName in new string[] { "instance1", "instance2" })
        {
            var config = _registry.CreateConfiguration(c);

            // Form
            /*
            var form = _formGenerator.Generate(config);
            form.Visit(new ConsoleFormVisitor());
            form.Populate(config);

            
            var result = c.ValidateConfiguration(config);

            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var err in result.Errors)
            {
                Console.WriteLine($"ERROR: {err.Key}: {err.Value}");
            }
            Console.ForegroundColor = ConsoleColor.White;
            */

            _registry.CreateInstance(c, instanceName, config);
        }

        _registry.Start();

        Console.ReadLine();

        _registry.Stop();

        Console.WriteLine("Slipstream stopped");
    }
}