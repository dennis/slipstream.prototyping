using Slipstream.Domain;
using Slipstream.Domain.Triggers;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Infrastructure;

public class TriggerContainer : ITriggerContainer
{
    private readonly Dictionary<string, ITriggerFactory> _types = new();

    public IEnumerable<string> Keys => _types.Keys;
    public IDictionary<string, ITriggerFactory> Types => _types;
    public IList<ITrigger> Triggers { get; } = new List<ITrigger>();

    public TriggerContainer(IEnumerable<ITriggerFactory> factories)
    {
        foreach (var item in factories)
        {
            Add(item.TypeName, item);
        }
    }

    public ITriggerFactory this[string key]
        => _types[key];

    public void Add(EntityTypeName typeName, ITriggerFactory factory)
    {
        _types.Add(typeName, factory);
    }
}
