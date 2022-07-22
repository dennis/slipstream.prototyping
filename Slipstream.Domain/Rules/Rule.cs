using Slipstream.Domain.Actions;
using Slipstream.Domain.Triggers;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Domain.Rules;

public class Rule : IRule
{
    public EntityName Name { get; }
    public EntityTypeName TypeName => "rule";
    public IRuleConfiguration Configuration { get; }

    public ITrigger Trigger { get; }
    public IAction Action { get; }

    public Rule(IRegistry registry, EntityName name, IRuleConfiguration configuration)
    {
        Name = name;
        Trigger = registry.TriggerContainer.Triggers.Single(a => a.Name == configuration.Trigger);
        Action = registry.ActionContainer.Actions.Single(a => a.Name == configuration.Action);
        Configuration = configuration;
    }
}
