using Slipstream.Domain.ValueObjects;

namespace Slipstream.Domain;

public interface IApplicationSettings
{
    IEnumerable<(EntityName, EntityName)> ReadInstances();
    void SaveInstance(EntityName pluginName, EntityName instanceName, string content);
    string LoadInstance(EntityName pluginName, EntityName instanceName);
}
