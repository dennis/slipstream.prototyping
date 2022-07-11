using Slipstream.Domain.Configuration;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Domain.Entities;

public interface IPlugin : IEntity
{
    IEnumerable<EntityName> InstanceNames { get; }

    ConfigurationValidationResult ValidateConfiguration(IConfiguration config);

    void CreateInstance(EntityName instanceName, IConfiguration config);

    IConfiguration CreateConfiguration();
}
