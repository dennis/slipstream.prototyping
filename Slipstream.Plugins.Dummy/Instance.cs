using FluentValidation;

using Slipstream.Domain.Attributes;
using Slipstream.Domain.Entities;
using Slipstream.Domain.Events;
using Slipstream.Domain.Instances;
using Slipstream.Domain.ValueObjects;
using Slipstream.Plugins.Dummy.Events;

using System.Text.Json;

namespace Slipstream.Plugins.Dummy;

internal class InstanceConfigurationValidator : AbstractValidator<InstanceConfiguration>
{
    public InstanceConfigurationValidator()
    {
        RuleFor(x => x.String).NotEmpty();
    }
}

public class InstanceConfiguration : IInstanceConfiguration
{
    [FormHelp("Type any string you like.")]
    public string String { get; set; } = "";
    public int Integer { get; set; }
}

public class InstanceFactory : IInstanceFactory
{
    private readonly IEventPublisher _eventPublisher;

    public EntityTypeName TypeName => "Dummy";

    public InstanceFactory(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public IInstanceConfiguration? ConfigurationJsonDecoder(string json)
        => JsonSerializer.Deserialize<InstanceConfiguration>(json);

    public string ConfigurationJsonEncoder(IInstanceConfiguration? config)
        => JsonSerializer.Serialize((InstanceConfiguration?)config);

    public IInstance Create(EntityName name, IInstanceConfiguration? config)
    {
        if(config == null)
            throw new ArgumentNullException(nameof(config));

        return new Instance(name, (InstanceConfiguration)config, this);
    }

    public IInstanceConfiguration? CreateConfiguration()
        => new InstanceConfiguration();

    public ConfigurationValidation Validate(IInstanceConfiguration config)
    {
        var result = new InstanceConfigurationValidator().Validate((InstanceConfiguration)config);

        return ConfigurationValidation.FromFluentValidationResult(result);
    }

    internal Task StartTaskAsync(CancellationToken cancel)
    {
        return Task.Run(async () =>
        {
            while (!cancel.IsCancellationRequested)
            {
                var c = Console.ReadKey().KeyChar;

                Console.WriteLine("GOT CHAR: " + c);

                await _eventPublisher.Publish(new KeyPressEvent(c)).ConfigureAwait(false);
            }
        }, cancel);
    }
}

public class Instance : IInstance
{
    private readonly InstanceConfiguration _configuration;
    private readonly InstanceFactory _factory;

    public EntityName Name { get; private set; }
    public EntityTypeName TypeName => EntityTypeName.From("Dummy");

    public IInstanceConfiguration? Configuration => _configuration;

    public Instance(EntityName name, InstanceConfiguration config, InstanceFactory factory)
        => (Name, _configuration, _factory) = (name, config, factory);

    internal void OnKeyPressEvent(KeyPressEvent @event)
    {
        Console.WriteLine($"  [{Name}] Got KeyPress '{@event.Key}'");
    }

    public Task MainAsync(CancellationToken cancel)
        => _factory.StartTaskAsync(cancel);
}
