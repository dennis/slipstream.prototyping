using Slipstream.Domain;
using Slipstream.Domain.Configuration;
using Slipstream.Domain.Entities;
using Slipstream.Domain.ValueObjects;

namespace Slipstream.Infrastructure;

public class Registry : IRegistry
{
    private readonly Dictionary<EntityName, IPlugin> _plugins = new();
    private readonly Dictionary<EntityName, Task> _tasks = new();  // TODO - we need to handle it

    private CancellationTokenSource _cancelTokenSource;

    private bool _started;

    public IEnumerable<IPlugin> Plugins { get => _plugins.Values; }

    public Registry(IEnumerable<IPlugin> plugins)
    {
        _cancelTokenSource = new CancellationTokenSource();

        foreach (var plugin in plugins)
        {
            AddPlugin(plugin);
        }
    }

    public void Start()
    {
        if (_started)
            throw new InvalidOperationException("Already started");

        _cancelTokenSource = new CancellationTokenSource();

        foreach (var plugin in Plugins)
        {
            _tasks.Add(plugin.Name, plugin.MainAsync(_cancelTokenSource.Token));
        }

        _started = true;
    }

    public void Stop()
    {
        _cancelTokenSource.Cancel();
        _started = false;
    }

    private void AddPlugin(IPlugin plugin)
    {
        EnsureValidEntityName(plugin.Name);

        _plugins.Add(plugin.Name, plugin);
    }

    public IPlugin? GetPlugin(EntityName name)
    {
        if (_plugins.TryGetValue(name, out var plugin))
        {
            return plugin;
        }
        return null;
    }

    public void CreateInstance(IPlugin plugin, EntityName instanceName, IInstanceConfiguration config)
    {
        EnsureValidEntityName(instanceName);

        plugin.CreateInstance(instanceName, config);
    }

    private void EnsureValidEntityName(EntityName name)
    {
        if (_plugins.ContainsKey(name))
        {
            throw new ArgumentException($"{name} already exists");
        }

        foreach (var plugin in _plugins.Values)
        {
            if (plugin.InstanceNames.Any(a => a == name))
            {
                throw new ArgumentException($"{name} already exists");
            }
        }
    }
}
