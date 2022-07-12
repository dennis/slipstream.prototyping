using Slipstream.Domain.Configuration;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Domain.Entities;

public interface IPlugin : IEntity
{
    IEnumerable<EntityName> InstanceNames { get; }

    InstanceConfigurationValidationResult ValidateInstanceConfiguration(IInstanceConfiguration config);

    void CreateInstance(EntityName instanceName, IInstanceConfiguration config);

    IInstanceConfiguration CreateInstanceConfiguration();
}
