using Slipstream.Domain.Actions;
using Slipstream.Domain.Entities;
using Slipstream.Domain.ValueObjects;

using System.Text.Json;

namespace Slipstream.Plugins.Dummy;

internal class PrintActionFactory : IActionFactory
{
    public EntityTypeName TypeName => "printaction";

    public IActionConfiguration? ConfigurationJsonDecoder(string json)
        => JsonSerializer.Deserialize<PrintActionConfiguration>(json);

    public string ConfigurationJsonEncoder(IActionConfiguration? config)
        => JsonSerializer.Serialize(config);

    public IAction Create(EntityName name, IActionConfiguration? config)
    {
        if (config == null) throw new ArgumentNullException(nameof(config));
        return new PrintAction(name, config);
    }

    public IActionConfiguration? CreateConfiguration()
        => new PrintActionConfiguration();

    public ConfigurationValidation Validate(IActionConfiguration? config)
        => ConfigurationValidation.OK;
}
