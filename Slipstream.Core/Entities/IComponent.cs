using Slipstream.Core.Configuration;

namespace Slipstream.Core.Entities;

public interface IComponent : IEntity
{
    ConfigurationValidationResult ValidateConfiguration(IConfiguration config);
}
