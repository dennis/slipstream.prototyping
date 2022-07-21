using Slipstream.Domain;
using Slipstream.Domain.Actions;
using Slipstream.Domain.Triggers;

namespace Slipstream.Infrastructure;

public class ActionContainer : IActionContainer
{
    public IList<IAction> Actions { get; } = new List<IAction>();
    public IDictionary<string, IActionFactory> Types { get; } = new Dictionary<string, IActionFactory>();

    public ActionContainer(IEnumerable<IActionFactory> factories)
    {
        foreach (var item in factories)
        {
            Types.Add(item.TypeName, item);
        }
    }
}
