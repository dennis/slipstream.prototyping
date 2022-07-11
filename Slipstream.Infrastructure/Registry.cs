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
    private readonly Dictionary<EntityName, ComponentData> _components = new();
    private readonly Dictionary<EntityName, Task> _tasks = new();  // TODO - we need to handle it

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IServiceScope _scope;
    private CancellationTokenSource _cancelTokenSource;

    private bool _started;

    public IEnumerable<IComponent> Components { get => _components.Values.Select(a => a.Component); }

    public Registry(IEnumerable<IComponent> components, IServiceScopeFactory serviceScopeFactory)
    {
        _cancelTokenSource = new CancellationTokenSource();
        _serviceScopeFactory = serviceScopeFactory;
        _scope = _serviceScopeFactory.CreateScope();


        foreach (var component in components)
        {
            var componentType = component.GetType();
            var meta = componentType.GetCustomAttribute<SlipstreamComponent>();
            if (meta is null)
            {
                Console.WriteLine($"{componentType.FullName} got no SlipstreamComponent description. Skipping");
                continue;
            }

            AddComponent(meta, component);
        }
    }

    public void Start()
    {
        if (_started)
            throw new InvalidOperationException("Already started");

        _cancelTokenSource = new CancellationTokenSource();

        foreach (var component in Components)
        {
            _tasks.Add(component.Name, component.MainAsync(_cancelTokenSource.Token));
        }

        _started = true;
    }

    public void Stop()
    {
        _cancelTokenSource.Cancel();
        _started = false;
    }

    private void AddComponent(SlipstreamComponent meta, IComponent component)
    {
        EnsureValidEntityName(component.Name);

        _components.Add(component.Name, new ComponentData(
            Component: component,
            InstanceFactoryType: meta.InstanceFactoryType,
            ConfigurationType: meta.ConfigurationType
        ));
    }

    public IComponent GetComponent(EntityName name)
    {
        return _components[name].Component;
    }

    public void CreateInstance(IComponent component, EntityName instanceName, IConfiguration config)
    {
        EnsureValidEntityName(instanceName);

        var instanceFactory = (IInstanceFactory)_scope.ServiceProvider.GetRequiredService(_components[component.Name].InstanceFactoryType);

        var instance = instanceFactory.Create(instanceName, config);

        _components[component.Name].Component.AddInstance(instanceName, instance);
    }

    private void EnsureValidEntityName(EntityName name)
    {
        if (_components.ContainsKey(name))
        {
            throw new ArgumentException($"{name} already exists");
        }

        foreach (var componentData in _components.Values)
        {
            if (componentData.Component.InstanceNames.Any(a => a == name))
            {
                throw new ArgumentException($"{name} already exists");
            }
        }
    }

    public IConfiguration CreateConfiguration(IComponent component)
    {
        return (IConfiguration)_scope.ServiceProvider.GetRequiredService(_components[component.Name].ConfigurationType);
    }

    private record ComponentData(IComponent Component, Type InstanceFactoryType, Type ConfigurationType)
    {
    }
}
