using Microsoft.Extensions.DependencyInjection;

using Slipstream.Core.Attributes;
using Slipstream.Core.Configuration;
using Slipstream.Core.Entities;
using Slipstream.Core.ValueObjects;

using System.Reactive.Linq;
using System.Reflection;

namespace Slipstream.Core;

public class Registry : IRegistry
{
    private readonly Dictionary<EntityName, ComponentData> _components = new();
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private CancellationTokenSource _cancelTokenSource;
    private IObservable<IEvent> _events;
    private bool _started;

    public IEnumerable<IComponent> Components { get => _components.Values.Select(a => a.Component); }
    public IEnumerable<IInstance> Instances { get => _components.Values.SelectMany(a => a.Instances.Values);  }

    public Registry(IEnumerable<IComponent> components, IServiceScopeFactory serviceScopeFactory)
    {
        _cancelTokenSource = new CancellationTokenSource();
        _serviceScopeFactory = serviceScopeFactory;

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

        // Just to populate _events
        _events = Observable.Merge(Components
            .Select(a => a.Output(_cancelTokenSource.Token))
            .Where(a => a is not null)
            .Select(a => a!));
    }

    public void Start()
    {
        if (_started)
            throw new InvalidOperationException("Already started");

        _cancelTokenSource = new CancellationTokenSource();

        var entities = new List<IEntity>();
        entities.AddRange(Components);
        entities.AddRange(Instances);

        // Collect all the outputs into one, we can use
        _events = Observable.Merge(entities
            .Select(a => a.Output(_cancelTokenSource.Token))
            .Where(a => a is not null)
            .Select(a => a!));

        // Add this as input, so everybody gets the events
        entities.ForEach(a => a.Input(_events, _cancelTokenSource.Token));

        _events.Wait();
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
            ConfigurationType: meta.ConfigurationType,
            Instances: new Dictionary<EntityName, IInstance>())
        );
    }

    public IComponent GetComponent(EntityName name)
    {
        return _components[name].Component;
    }

    public void CreateInstance(IComponent component, EntityName instanceName, IConfiguration config)
    {
        EnsureValidEntityName(instanceName);

        using var scope = _serviceScopeFactory.CreateScope();
        var instanceFactory = (IInstanceFactory)scope.ServiceProvider.GetRequiredService(_components[component.Name].InstanceFactoryType);

        var instance = instanceFactory.Create(instanceName, config);

        _components[component.Name].Instances.Add(instanceName, instance);
    }

    private void EnsureValidEntityName(EntityName name)
    {
        if (_components.ContainsKey(name))
        {
            throw new ArgumentException($"{name} already exists");
        }

        foreach (var component in _components.Values)
        {
            if (component.Instances.ContainsKey(name))
            {
                throw new ArgumentException($"{name} already exists");
            }
        }
    }

    public IConfiguration CreateConfiguration(IComponent component)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        return (IConfiguration)scope.ServiceProvider.GetRequiredService(_components[component.Name].ConfigurationType);
    }

    private record ComponentData(IComponent Component, Type InstanceFactoryType, Type ConfigurationType, Dictionary<EntityName, IInstance> Instances)
    {
    }
}
