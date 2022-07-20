using Slipstream.Domain.Entities;
using Slipstream.Domain.Events;
using Slipstream.Domain.Instances;
using Slipstream.Domain.ValueObjects;
using Slipstream.Plugins.Dummy.Events;

using System.Text.Json;

namespace Slipstream.Plugins.Dummy;

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

    public ConfigurationValidation Validate(IInstanceConfiguration? config)
    {
        if (config == null)
            throw new ArgumentNullException(nameof(config));

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

                await _eventPublisher.PublishAsync(new KeyPressEvent(c)).ConfigureAwait(false);
            }
        }, cancel);
    }
}
