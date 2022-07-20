using Slipstream.Domain.Instances;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Domain;

public interface IInstanceContainer
{
    IList<IInstance> Instances { get; }

    IInstanceFactory this[string key] { get; }

    IEnumerable<string> Keys { get; }

    void Add(EntityTypeName typeName, IInstanceFactory factory);
}