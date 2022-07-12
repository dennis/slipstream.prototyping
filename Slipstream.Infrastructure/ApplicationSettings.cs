using Slipstream.Domain;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Infrastructure;

public class ApplicationSettings : IApplicationSettings
{
    public ApplicationSettings()
    {
        Directory.CreateDirectory("save/");
        Directory.CreateDirectory("save/instances/");
    }

    public void SaveInstance(EntityName pluginName, EntityName instanceName, string content)
    {
        Directory.CreateDirectory($"save/instances/{pluginName}");

        File.WriteAllText($"save/instances/{pluginName}/{instanceName}.json", content);
    }

    public IEnumerable<(EntityName, EntityName)> ReadInstances()
    {
        var entries = new List<(EntityName, EntityName)>();

        foreach (var pluginDirectory in Directory.GetDirectories("save/instances/"))
        {
            var pluginName = Path.GetFileName(pluginDirectory);
            foreach (var instanceFile in Directory.GetFiles(pluginDirectory))
            {
                var instanceName = Path.GetFileNameWithoutExtension(Path.GetFileName(instanceFile));
                entries.Add((pluginName, instanceName));
            }
        }

        return entries;
    }

    public string LoadInstance(EntityName pluginName, EntityName instanceName)
    {
        return File.ReadAllText($"save/instances/{pluginName}/{instanceName}.json");
    }
}
