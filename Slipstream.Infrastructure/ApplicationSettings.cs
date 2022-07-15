using Slipstream.Domain;
using Slipstream.Domain.Instances;
using Slipstream.Domain.Triggers;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Infrastructure;

public class ApplicationSettings : IApplicationSettings
{
    private readonly IRegistry _registry;

    public ApplicationSettings(IRegistry registry)
    {
        Directory.CreateDirectory("save/");
        Directory.CreateDirectory("save/instances/");
        Directory.CreateDirectory("save/triggers/");

        _registry = registry;
    }

    public void SaveInstance(IInstance instance)
    {
        var json = _registry.AvailableInstanceTypes[instance.TypeName].ConfigurationJsonEncoder(instance.Configuration);
        SaveEntity("save/instances", (string)instance.TypeName, instance.Name, json);
    }
        
    public IEnumerable<(EntityTypeName, EntityName)> ReadInstances()
        => ReadEntities("save/instances");

    public string LoadInstance(EntityTypeName entityTypeName, EntityName entityName)
        => LoadEntity("save/instances/", entityTypeName, entityName);

    public void SaveTrigger(ITrigger trigger)
    {
        var json = _registry.AvailableTriggerTypes[trigger.TypeName].ConfigurationJsonEncoder(trigger.Configuration);
        SaveEntity("save/triggers", (string)trigger.TypeName, trigger.Name, json);
    }

    public IEnumerable<(EntityTypeName, EntityName)> ReadTriggers()
        => ReadEntities("save/triggers");

    public string LoadTrigger(EntityTypeName entityTypeName, EntityName entityName)
        => LoadEntity("save/triggers/", entityTypeName, entityName);

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
