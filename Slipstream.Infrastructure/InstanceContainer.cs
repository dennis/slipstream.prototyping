using Slipstream.Domain;
using Slipstream.Domain.Instances;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Infrastructure;

public class InstanceContainer : IInstanceContainer
{
    private readonly IDictionary<string, IInstanceFactory> _types = new Dictionary<string, IInstanceFactory>();

    public IEnumerable<string> Keys => _types.Keys;

    public IList<IInstance> Instances { get; } = new List<IInstance>();

    public InstanceContainer(IEnumerable<IInstanceFactory> factories)
    {
        foreach (var item in factories)
        {
            Add(item.TypeName, item);
        }
    }

    public IInstanceFactory this[string key]
        => _types[key];

    public void Add(EntityTypeName typeName, IInstanceFactory factory)
    {
        _types.Add(typeName, factory);
    }
}
