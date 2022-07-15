using Slipstream.Domain.Instances;
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
}
