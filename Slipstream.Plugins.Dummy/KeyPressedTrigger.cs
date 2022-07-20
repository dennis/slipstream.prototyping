using FluentValidation;

using Slipstream.Domain.Entities;
using Slipstream.Domain.Events;
using Slipstream.Domain.Triggers;
using Slipstream.Domain.ValueObjects;
using Slipstream.Plugins.Dummy.Events;

using System.Text.Json;

namespace Slipstream.Plugins.Dummy;

public class KeyPressTriggerConfigurationValidator : AbstractValidator<KeyPressTriggerConfiguration>
{
    public KeyPressTriggerConfigurationValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty()
            .MaximumLength(1);
    }
}

public class KeyPressTriggerConfiguration : ITriggerConfiguration
{
    public string Key { get; set; } = "";
}

public class KeyPressTriggerFactory : ITriggerFactory
{
    public EntityTypeName TypeName => EntityTypeName.From("KeyPress");

    public ITriggerConfiguration? ConfigurationJsonDecoder(string json)
        => JsonSerializer.Deserialize<KeyPressTriggerConfiguration>(json);

    public string ConfigurationJsonEncoder(ITriggerConfiguration? config)
        => JsonSerializer.Serialize((KeyPressTriggerConfiguration?)config);

    public ITrigger Create(EntityName name, ITriggerConfiguration? setup)
        => new KeyPressedTrigger(name, (KeyPressTriggerConfiguration?)setup);

    public ITriggerConfiguration? CreateConfiguration()
        => new KeyPressTriggerConfiguration();

    public ConfigurationValidation Validate(ITriggerConfiguration config)
    {
        var result = new KeyPressTriggerConfigurationValidator().Validate((KeyPressTriggerConfiguration)config);

        return ConfigurationValidation.FromFluentValidationResult(result);
    }
}

public class KeyPressedTrigger : ITrigger
{
    private readonly KeyPressTriggerConfiguration _configuration;

    public EntityName Name { get; init; }
    public EntityTypeName TypeName => EntityTypeName.From("KeyPress");
    public ITriggerConfiguration? Configuration => _configuration;

    public KeyPressedTrigger(EntityName name, KeyPressTriggerConfiguration configuration)
        => (Name, _configuration) = (name, configuration);

    // Accepts() evaluates everything, so we just return true
    public Task<bool> EvaluateAsync(IEvent @event)
        => Task.FromResult(true);

    public bool Accepts(IEvent @event)
        => @event is KeyPressEvent event1 && _configuration.Key.Contains(event1.Key);
}
