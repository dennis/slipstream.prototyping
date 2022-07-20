using Slipstream.Domain.Triggers;

namespace Slipstream.Domain;

public interface ITriggerContainer
{
    IDictionary<string, ITriggerFactory> Types { get; }
    IList<ITrigger> Triggers { get; }
}