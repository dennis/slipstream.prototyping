using Slipstream.Domain.Configuration;
using Slipstream.Domain.Entities;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Domain;

public interface IRegistry
{
    public IEnumerable<IPlugin> Plugins { get; }

    public IPlugin? GetPlugin(EntityName name);

    public void CreateInstance(IPlugin plugin, EntityName instanceName, IConfiguration config);

    public IConfiguration CreateConfiguration(IPlugin plugin);

    public void Start();
    public void Stop();
}