using Slipstream.Domain.Actions;
using Slipstream.Domain.Instances;
using Slipstream.Domain.Rules;
using Slipstream.Domain.Triggers;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Domain;

public interface IApplicationSettings
{
    IEnumerable<(EntityTypeName, EntityName)> ReadInstances();
    void SaveInstance(IInstance instance);
    string LoadInstance(EntityTypeName pluginName, EntityName instanceName);

    IEnumerable<(EntityTypeName, EntityName)> ReadTriggers();
    void SaveTrigger(ITrigger trigger);
    string LoadTrigger(EntityTypeName triggerTypeName, EntityName triggerName);

    IEnumerable<(EntityTypeName, EntityName)> ReadRules();
    void SaveRule(IRule rule);
    public string LoadRule(EntityName entityName);

    IEnumerable<(EntityTypeName, EntityName)> ReadActions();
    void SaveAction(IAction action);
    public string LoadAction(EntityTypeName triggerTypeName, EntityName entityName);
    void Load(Action<string> print, Action<string> error);
    void Save(Action<string> print);
}
