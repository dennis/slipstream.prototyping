using Microsoft.Extensions.DependencyInjection;

using Slipstream.Domain.Entities;
using Slipstream.Domain.ValueObjects;

using System.Text.Json;

namespace Slipstream.Domain.Rules;

public class RuleRactory : IRuleFactory
{
    private readonly IServiceScope _scope;

    public RuleRactory(IServiceScopeFactory serviceScopeFactory)
    {
        _scope = serviceScopeFactory.CreateScope();
    }

    public EntityTypeName TypeName => throw new NotImplementedException();

    public IRuleConfiguration? ConfigurationJsonDecoder(string json)
        => JsonSerializer.Deserialize<RuleConfiguration>(json);

    public string ConfigurationJsonEncoder(IRuleConfiguration? config)
        => JsonSerializer.Serialize(config);

    public IRule Create(EntityName name, IRuleConfiguration? config)
    {
        if (config == null) throw new ArgumentNullException(nameof(config));
        return new Rule(_scope.ServiceProvider.GetRequiredService<IRegistry>(), name, config);
    }

    public IRuleConfiguration? CreateConfiguration()
        => new RuleConfiguration();

    public ConfigurationValidation Validate(IRuleConfiguration? config)
        => new();
}
