using Slipstream.Domain.Configuration;
using Slipstream.Domain.Entities;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Domain;

public interface IRegistry
{
    public IEnumerable<IComponent> Components { get; }

    public IComponent GetComponent(EntityName name);

    public void CreateInstance(IComponent component, EntityName instanceName, IConfiguration config);

    public IConfiguration CreateConfiguration(IComponent component);

    public void Start();
    public void Stop();
}