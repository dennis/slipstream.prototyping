using Slipstream.Domain.Configuration;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Domain.Entities;

public interface IPlugin : IEntity
{
    IEnumerable<EntityName> InstanceNames { get; }

    void AddInstance(EntityName instanceName, IInstance instance);

    ConfigurationValidationResult ValidateConfiguration(IConfiguration config);
}
