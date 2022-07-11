using Microsoft.Extensions.DependencyInjection;

using Slipstream.Domain;
using Slipstream.Domain.Attributes;
using Slipstream.Domain.Configuration;
using Slipstream.Domain.Entities;
using Slipstream.Domain.ValueObjects;

using System.Reflection;

namespace Slipstream.Infrastructure;

public class Registry : IRegistry
{
    private readonly Dictionary<EntityName, PluginData> _plugins = new();
    private readonly Dictionary<EntityName, Task> _tasks = new();  // TODO - we need to handle it

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IServiceScope _scope;
    private CancellationTokenSource _cancelTokenSource;

    private bool _started;

    public IEnumerable<IPlugin> Plugins { get => _plugins.Values.Select(a => a.Plugin); }

    public Registry(IEnumerable<IPlugin> plugins, IServiceScopeFactory serviceScopeFactory)
    {
        _cancelTokenSource = new CancellationTokenSource();
        _serviceScopeFactory = serviceScopeFactory;
        _scope = _serviceScopeFactory.CreateScope();

        foreach (var plugin in plugins)
        {
            var pluginType = plugin.GetType();
            var meta = pluginType.GetCustomAttribute<SlipstreamPlugin>();
            if (meta is null)
            {
                Console.WriteLine($"{pluginType.FullName} got no SlipstreamPlugin description. Skipping");
                continue;
            }

            AddPlugin(meta, plugin);
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

    private void AddPlugin(SlipstreamPlugin meta, IPlugin plugin)
    {
        EnsureValidEntityName(plugin.Name);

        _plugins.Add(plugin.Name, new PluginData(
            Plugin: plugin,
            InstanceFactoryType: meta.InstanceFactoryType,
            ConfigurationType: meta.ConfigurationType
        ));
    }

    public IPlugin? GetPlugin(EntityName name)
    {
        if (_plugins.TryGetValue(name, out var pluginData))
        {
            return pluginData.Plugin;
        }
        return null;
    }

    public void CreateInstance(IPlugin plugin, EntityName instanceName, IConfiguration config)
    {
        EnsureValidEntityName(instanceName);

        var instanceFactory = (IInstanceFactory)_scope.ServiceProvider.GetRequiredService(_plugins[plugin.Name].InstanceFactoryType);

        var instance = instanceFactory.Create(instanceName, config);

        _plugins[plugin.Name].Plugin.AddInstance(instanceName, instance);
    }

    private void EnsureValidEntityName(EntityName name)
    {
        if (_plugins.ContainsKey(name))
        {
            throw new ArgumentException($"{name} already exists");
        }

        foreach (var pluginData in _plugins.Values)
        {
            if (pluginData.Plugin.InstanceNames.Any(a => a == name))
            {
                throw new ArgumentException($"{name} already exists");
            }
        }
    }

    public IConfiguration CreateConfiguration(IPlugin plugin)
    {
        return (IConfiguration)_scope.ServiceProvider.GetRequiredService(_plugins[plugin.Name].ConfigurationType);
    }

    private record PluginData(IPlugin Plugin, Type InstanceFactoryType, Type ConfigurationType)
    {
    }
}
