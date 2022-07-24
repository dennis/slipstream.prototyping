using Slipstream.Domain;
using Slipstream.Domain.Actions;
using Slipstream.Domain.Instances;
using Slipstream.Domain.Rules;
using Slipstream.Domain.Triggers;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Infrastructure;

public class ApplicationSettings : IApplicationSettings
{
    private readonly IRegistry _registry;
    private readonly IRuleFactory _ruleFactory;
    private readonly string _savePath = "slipstream.setup";

    public ApplicationSettings(IRegistry registry, IRuleFactory ruleFactory)
    {
        _registry = registry;
        _ruleFactory = ruleFactory;
    }

    public void Load(Action<string> print, Action<string> error)
    {
        print("Loading");

        foreach (var line in File.ReadAllLines(_savePath))
        {
            var elements = line.Split('\t');

            switch (elements[0])
            {
                case "instances":
                    ParseInstanceLine(print, error, elements[1], elements[2], elements[3]);
                    break;
                case "triggers":
                    ParseTriggersLine(print, error, elements[1], elements[2], elements[3]);
                    break;
                case "actions":
                    ParseActionsLine(print, error, elements[1], elements[2], elements[3]);
                    break;
                case "rules":
                    ParseRulesLine(print, elements[2], elements[3]);
                    break;
                default:
                    error("Ignoring line: " + line);
                    break;
            }
        }


        print("Done");
    }

    private void ParseRulesLine(Action<string> print, string entityName, string json)
    {
        var config = _ruleFactory.ConfigurationJsonDecoder(json);
        var rule = _ruleFactory.Create(entityName, config);

        _registry.AddRule(rule);

        print($" - rule: {entityName}");
    }

    private void ParseActionsLine(Action<string> print, Action<string> error, string entityTypeName, string entityName, string json)
    {
        var factory = _registry.ActionContainer.Types[entityTypeName];

        var config = factory.ConfigurationJsonDecoder(json);
        if (config is not null && !factory.Validate(config).IsValid())
        {
            error($" - action: {entityTypeName} / {entityName} - configuration contains error. Ignoring");
            return;
        }

        var action = factory.Create(entityName, config);

        _registry.AddAction(action);

        print($" - action: {entityTypeName} / {entityName}");
    }

    private void ParseTriggersLine(Action<string> print, Action<string> error, string entityTypeName, string entityName, string json)
    {
        var factory = _registry.TriggerContainer.Types[entityTypeName];

        var config = factory.ConfigurationJsonDecoder(json);
        if (config is not null && !factory.Validate(config).IsValid())
        {
            error($" - trigger: {entityTypeName} / {entityName} - configuration contains error. Ignoring");
            return;
        }

        var trigger = factory.Create(entityName, config);

        _registry.AddTrigger(trigger);

        print($" - trigger: {entityTypeName} / {entityName}");
    }

    private void ParseInstanceLine(Action<string> print, Action<string> error, string entityTypeName, string entityName, string json)
    {
        var factory = _registry.InstanceContainer[entityTypeName];

        var config = factory.ConfigurationJsonDecoder(json);
        if (config is not null && !factory.Validate(config).IsValid())
        {
            error($" - instance: {entityTypeName} / {entityName} - configuration contains error. Ignoring");
            return;
        }
        var instance = factory.Create(entityName, config);

        _registry.AddInstance(instance);

        print($" - instance: {entityTypeName} / {entityName}");
    }

    public void Save(Action<string> print)
    {
        if (File.Exists(_savePath))
        {
            File.Move(_savePath, Path.GetFileNameWithoutExtension(_savePath) + "-old" + Path.GetExtension(_savePath), true);
        }
        print("Saving...");

        foreach (var instance in _registry.InstanceContainer.Instances)
        {
            SaveInstance(instance);
        }

        foreach (var trigger in _registry.TriggerContainer.Triggers)
        {
            SaveTrigger(trigger);
        }

        foreach (var action in _registry.ActionContainer.Actions)
        {
            SaveAction(action);
        }

        foreach (var rule in _registry.RuleContainer.Rules)
        {
            SaveRule(rule);
        }
    }

    private void SaveInstance(IInstance instance)
    {
        var json = _registry.InstanceContainer[instance.TypeName].ConfigurationJsonEncoder(instance.Configuration);
        SaveEntity("instances", (string)instance.TypeName, instance.Name, json);
    }

    private void SaveTrigger(ITrigger trigger)
    {
        var json = _registry.TriggerContainer.Types[trigger.TypeName].ConfigurationJsonEncoder(trigger.Configuration);
        SaveEntity("triggers", (string)trigger.TypeName, trigger.Name, json);
    }

    private void SaveRule(IRule rule)
    {
        var json = _ruleFactory.ConfigurationJsonEncoder(rule.Configuration);
        SaveEntity("rules", (string)rule.TypeName, rule.Name, json);
    }

    private void SaveAction(IAction action)
    {
        var json = _registry.ActionContainer.Types[action.TypeName].ConfigurationJsonEncoder(action.Configuration); 
        SaveEntity("actions", (string)action.TypeName, action.Name, json);
    }

    private void SaveEntity(string type, EntityTypeName entityTypeName, EntityName entityName, string content)
    {
        File.AppendAllLines(_savePath, new string[] { $"{type}\t{entityTypeName}\t{entityName}\t{content}" });
    }
}
