using Slipstream.Domain.Actions;

namespace Slipstream.Domain;

public interface IActionContainer
{
    IDictionary<string, IActionFactory> Types { get; }
    IList<IAction> Actions { get; }
}