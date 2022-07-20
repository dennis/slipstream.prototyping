
using Slipstream.Domain.Entities;
using Slipstream.Domain.Triggers;
using Slipstream.Domain.ValueObjects;

using System.Text.Json;

namespace Slipstream.Plugins.Dummy;

public class KeyPressTriggerFactory : ITriggerFactory
{
    public EntityTypeName TypeName => EntityTypeName.From("KeyPress");

    public ITriggerConfiguration? ConfigurationJsonDecoder(string json)
        => JsonSerializer.Deserialize<KeyPressTriggerConfiguration>(json);

    public string ConfigurationJsonEncoder(ITriggerConfiguration? config)
        => JsonSerializer.Serialize((KeyPressTriggerConfiguration?)config);

    public ITrigger Create(EntityName name, ITriggerConfiguration? config)
    {
        if (config == null)
            throw new ArgumentNullException(nameof(config));

        return new KeyPressedTrigger(name, (KeyPressTriggerConfiguration)config);
    }

    public ITriggerConfiguration? CreateConfiguration()
        => new KeyPressTriggerConfiguration();

    public ConfigurationValidation Validate(ITriggerConfiguration? config)
    {
        if (config == null)
            throw new ArgumentNullException(nameof(config));

        var result = new KeyPressTriggerConfigurationValidator().Validate((KeyPressTriggerConfiguration)config);

        return ConfigurationValidation.FromFluentValidationResult(result);
    }
}
