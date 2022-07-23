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

    public ApplicationSettings(IRegistry registry, IRuleFactory ruleFactory)
    {
        Directory.CreateDirectory("save/");
        Directory.CreateDirectory("save/instances/");
        Directory.CreateDirectory("save/triggers/");
        Directory.CreateDirectory("save/actions/");

        _registry = registry;
        _ruleFactory = ruleFactory;
    }

    public void Load(Action<string> print, Action<string> error)
    {
        print("Loading instances");

        foreach (var (instanceTypeName, instanceName) in ReadInstances())
        {
            var factory = _registry.InstanceContainer[instanceTypeName];

            var config = factory.ConfigurationJsonDecoder(LoadInstance(instanceTypeName, instanceName));
            if (config is not null && !factory.Validate(config).IsValid())
            {
                error($" - {instanceTypeName} / {instanceName} - configuration contains error. Ignoring");
                continue;
            }
            var instance = factory.Create(instanceName, config);

            _registry.AddInstance(instance);

            print($" - {instanceTypeName} / {instanceName}");
        }

        print("Loading triggers");

        foreach (var (triggerTypeName, triggerName) in ReadTriggers())
        {
            var factory = _registry.TriggerContainer.Types[triggerTypeName];

            var config = factory.ConfigurationJsonDecoder(LoadTrigger(triggerTypeName, triggerName));
            if (config is not null && !factory.Validate(config).IsValid())
            {
                error($" - {triggerTypeName} / {triggerName} - configuration contains error. Ignoring");
                continue;
            }

            var trigger = factory.Create(triggerName, config);

            _registry.AddTrigger(trigger);

            print($" - {triggerTypeName} / {triggerName}");
        }

        print("Loading actions");

        foreach (var (entityTypeName, entityName) in ReadActions())
        {
            var factory = _registry.ActionContainer.Types[entityTypeName];

            var config = factory.ConfigurationJsonDecoder(LoadAction(entityTypeName, entityName));
            if (config is not null && !factory.Validate(config).IsValid())
            {
                error($" - {entityTypeName} / {entityName} - configuration contains error. Ignoring");
                continue;
            }

            var action = factory.Create(entityName, config);

            _registry.AddAction(action);

            print($" - {entityTypeName} / {entityName}");
        }

        print("Loading rules");

        foreach (var (_, ruleName) in ReadRules())
        {
            var config = _ruleFactory.ConfigurationJsonDecoder(LoadRule(ruleName));
            var rule = _ruleFactory.Create(ruleName, config);

            _registry.AddRule(rule);

            print($" - {ruleName}");
        }

        print("Done");
    }

    public void Save(Action<string> print)
    {
        print("Saving...");

        foreach (var instance in _registry.InstanceContainer.Instances)
        {
            SaveInstance(instance);
        }

        foreach (var trigger in _registry.TriggerContainer.Triggers)
        {
            SaveTrigger(trigger);
        }

        foreach (var rule in _registry.RuleContainer.Rules)
        {
            SaveRule(rule);
        }

        foreach (var action in _registry.ActionContainer.Actions)
        {
            SaveAction(action);
        }

        print("Done");
    }

    private void SaveInstance(IInstance instance)
    {
        var json = _registry.InstanceContainer[instance.TypeName].ConfigurationJsonEncoder(instance.Configuration);
        SaveEntity("save/instances", (string)instance.TypeName, instance.Name, json);
    }

    private static IEnumerable<(EntityTypeName, EntityName)> ReadInstances()
        => ReadEntities("save/instances");

    private static string LoadInstance(EntityTypeName entityTypeName, EntityName entityName)
        => LoadEntity("save/instances/", entityTypeName, entityName);

    private void SaveTrigger(ITrigger trigger)
    {
        var json = _registry.TriggerContainer.Types[trigger.TypeName].ConfigurationJsonEncoder(trigger.Configuration);
        SaveEntity("save/triggers", (string)trigger.TypeName, trigger.Name, json);
    }

    private static IEnumerable<(EntityTypeName, EntityName)> ReadTriggers()
        => ReadEntities("save/triggers");

    private static string LoadTrigger(EntityTypeName entityTypeName, EntityName entityName)
        => LoadEntity("save/triggers/", entityTypeName, entityName);

    private void SaveRule(IRule rule)
    {
        var json = _ruleFactory.ConfigurationJsonEncoder(rule.Configuration);
        SaveEntity("save/rules", (string)rule.TypeName, rule.Name, json);
    }

    private static IEnumerable<(EntityTypeName, EntityName)> ReadRules()
        => ReadEntities("save/rules");

    private static string LoadRule(EntityName entityName)
        => LoadEntity("save/rules", "rule", entityName);

    private static IEnumerable<(EntityTypeName, EntityName)> ReadActions()
        => ReadEntities("save/actions");

    private void SaveAction(IAction action)
    {
        var json = _registry.ActionContainer.Types[action.TypeName].ConfigurationJsonEncoder(action.Configuration); 
        SaveEntity("save/actions", (string)action.TypeName, action.Name, json);
    }

    private static string LoadAction(EntityTypeName entityTypeName, EntityName entityName)
        => LoadEntity("save/actions", entityTypeName, entityName);

    private static string LoadEntity(string rootDirectory, EntityTypeName entityTypeName, EntityName entityName)
        => File.ReadAllText($"{rootDirectory}/{entityTypeName}/{entityName}.json");

    private static IEnumerable<(EntityTypeName, EntityName)> ReadEntities(string rootDirectory)
    {
        var entries = new List<(EntityTypeName, EntityName)>();

        foreach (var entityTypeDirectory in Directory.GetDirectories(rootDirectory))
        {
            var entityType = Path.GetFileName(entityTypeDirectory);
            foreach (var entityFilename in Directory.GetFiles(entityTypeDirectory))
            {
                var entityName = Path.GetFileNameWithoutExtension(Path.GetFileName(entityFilename));
                entries.Add((entityType, entityName));
            }
        }

        return entries;
    }

    private static void SaveEntity(string rootDirectory, EntityTypeName entityTypeName, EntityName entityName, string content)
    {
        Directory.CreateDirectory($"{rootDirectory}/{entityTypeName}");
        File.WriteAllText($"{rootDirectory}/{entityTypeName}/{entityName}.json", content);
    }

}
