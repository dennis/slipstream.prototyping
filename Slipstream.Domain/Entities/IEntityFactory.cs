using Slipstream.Domain.ValueObjects;

namespace Slipstream.Domain.Entities;

public interface IEntityFactory<out TEntity, TConfig> where TEntity : IEntity where TConfig : IEntityConfiguration
{
    EntityTypeName TypeName { get; }

    TEntity Create(EntityName name, TConfig? config);

    TConfig? CreateConfiguration();
    ConfigurationValidation Validate(TConfig? config);

    TConfig? ConfigurationJsonDecoder(string json);
    string ConfigurationJsonEncoder(TConfig? config);
}
