using Slipstream.Domain.Actions;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Plugins.Dummy;

public class PrintAction : IAction
{
    public EntityName Name { get; }
    public EntityTypeName TypeName => "printaction";

    public IActionConfiguration? Configuration { get; }

    public PrintAction(EntityName name, IActionConfiguration configuration)
        => (Name, Configuration) = (name, configuration);

    public void Invoke(object _)
    {
        Console.WriteLine(((PrintActionConfiguration)Configuration!).Message);
    }
}