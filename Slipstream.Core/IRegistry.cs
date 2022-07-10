using Slipstream.Core.Configuration;
using Slipstream.Core.Entities;
using Slipstream.Core.ValueObjects;

namespace Slipstream.Core;

public interface IRegistry
{
    public IEnumerable<IComponent> Components { get; }
    public IEnumerable<IInstance> Instances { get; }

    public IComponent GetComponent(EntityName name);

    public void CreateInstance(IComponent component, EntityName instanceName, IConfiguration config);

    public IConfiguration CreateConfiguration(IComponent component);

    public void Start();
    public void Stop();
}