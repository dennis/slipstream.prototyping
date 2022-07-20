using Slipstream.Domain;
using Slipstream.Domain.Entities;
using Slipstream.Domain.Forms;

namespace Slipstream.CLI;

internal class MainMenuHandler
{
    private readonly IRegistry _registry;
    private readonly IApplicationSettings _applicationSettings;
    private readonly TriggerMenuHandler _triggerMenuHandler;
    private readonly EntityHelper _entityHelper;

    public MainMenuHandler(
        IRegistry registry,
        IApplicationSettings applicationSettings,
        TriggerMenuHandler triggerMenuHandler,
        EntityHelper entityHelper
    )
    {
        _registry = registry;
        _applicationSettings = applicationSettings;
        _triggerMenuHandler = triggerMenuHandler;
        _entityHelper = entityHelper;
    }

    public void Show(TUIHelper tui)
    {
        bool quit = false;
        do
        {
            tui.PrintHeading("Main menu:")
                .Print(" 1 - list instance types")
                .Print(" 2 - create instance")
                .Print(" 3 - load config")
                .Print(" 4 - save config")
                .Print(" 5 - list instances")
                .Print(" t - triggers menu")
                .Print(" q - quit (and start slipstream)")
                .Spacer();

            var input = tui.ReadKey();

            switch (input)
            {
                case '1':
                    ShowInstanceTypes(tui);
                    break;

                case '5':
                    ShowInstances(tui);
                    break;

                case '2':
                    CreateInstance(tui);
                    break;

                case '3':
                    LoadConfig(tui);
                    break;

                case '4':
                    SaveConfig(tui);
                    break;

                case 't':
                case 'T':
                    _triggerMenuHandler.Show(tui.NewScope("triggers"));
                    break;

                case 'q':
                case 'Q':
                    quit = true;
                    break;

                default:
                    tui.Error("Unknown item");
                    break;
            }

            tui.Reset();
        } while (!quit);
    }

    private void ShowInstances(TUIHelper tui)
    {
        tui.PrintStrong("Instances available:");

        foreach (var instance in _registry.Instances)
        {
            tui.Print($" - {instance.Name}");
        }

        tui.Spacer();
    }

    private void ShowInstanceTypes(TUIHelper tui)
    {
        tui.PrintStrong("Instance types available:");

        foreach (var instanceType in _registry.AvailableInstanceTypes.Keys)
        {
            tui.Print($" - {instanceType}");
        }

        tui.Spacer();
    }

    private void CreateInstance(TUIHelper tui)
    {
        ShowInstanceTypes(tui);

        _entityHelper.Creator<IInstance, IInstanceFactory, IInstanceConfiguration>(
            tui.NewScope("new instance"), 
            (entityTypeName) => _registry.AvailableInstanceTypes.ContainsKey(entityTypeName),
            (entityTypeName) => _registry.AvailableInstanceTypes[entityTypeName].CreateConfiguration(),
            (entityTypeName, configuration) => _registry.AvailableInstanceTypes[entityTypeName].Validate(configuration),
            (entityTypeName, entityName, configuration) =>
            {
                var instance = _registry.AvailableInstanceTypes[entityTypeName].Create(entityName, configuration);
                _registry.AddInstance(instance);
            }
        );
    }

    private void LoadConfig(TUIHelper tui)
    {
        tui.PrintStrong("Loading instances");

        foreach (var (instanceTypeName, instanceName) in _applicationSettings.ReadInstances())
        {
            var factory = _registry.AvailableInstanceTypes[instanceTypeName];

            var config = factory.ConfigurationJsonDecoder(_applicationSettings.LoadInstance(instanceTypeName, instanceName));
            if (config is not null && !factory.Validate(config).IsValid())
            {
                tui.Error($" - {instanceTypeName} / {instanceName} - configuration contains error. Ignoring");
                continue;
            }
            var instance = factory.Create(instanceName, config);

            _registry.AddInstance(instance);

            tui.Print($" - {instanceTypeName} / {instanceName}");
        }

        tui.PrintStrong("Loading triggers");

        foreach (var (triggerTypeName, triggerName) in _applicationSettings.ReadTriggers())
        {
            var factory = _registry.AvailableTriggerTypes[triggerTypeName];

            var config = factory.ConfigurationJsonDecoder(_applicationSettings.LoadTrigger(triggerTypeName, triggerName));
            if (config is not null && !factory.Validate(config).IsValid())
            {
                tui.Error($" - {triggerTypeName} / {triggerName} - configuration contains error. Ignoring");
                continue;
            }

            var trigger = factory.Create(triggerName, config);

            _registry.AddTrigger(trigger);

            tui.Print($" - {triggerTypeName} / {triggerName}");
        }

        tui.Print("Done");
    }

    private void SaveConfig(TUIHelper tui)
    {
        tui.PrintStrong("Saving...");

        foreach (var instance in _registry.Instances)
        {
            _applicationSettings.SaveInstance(instance);
        }

        foreach (var trigger in _registry.Triggers)
        {
            _applicationSettings.SaveTrigger(trigger);
        }

        tui.Print("Done");
    }
}