using Slipstream.Domain.Entities;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Plugins.Dummy;

public class InstanceFactory : IInstanceFactory
{
    public IInstance Create<TConfig>(EntityName name, TConfig config)
    {
        return new Instance(name);
    }
}