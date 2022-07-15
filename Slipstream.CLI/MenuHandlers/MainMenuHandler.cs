using Slipstream.Domain;

namespace Slipstream.CLI.MenuHandlers;

internal class MainMenuHandler
{
    private readonly IRegistry _registry;
    private readonly IApplicationSettings _applicationSettings;
    private readonly TriggerMenuHandler _triggerMenuHandler;
    private readonly InstanceMenuHandler _instanceMenuHandler;
    private readonly EntityHelper _entityHelper;

    public MainMenuHandler(
        IRegistry registry,
        IApplicationSettings applicationSettings,
        TriggerMenuHandler triggerMenuHandler,
        InstanceMenuHandler instanceMenuHandler,
        EntityHelper entityHelper
    )
    {
        _registry = registry;
        _applicationSettings = applicationSettings;
        _triggerMenuHandler = triggerMenuHandler;
        _instanceMenuHandler = instanceMenuHandler;
        _entityHelper = entityHelper;
    }

    public void Show(TUIHelper tui)
    {
        bool quit = false;
        do
        {
            tui.PrintHeading("Main menu:")
                .Print(" 1 - load config")
                .Print(" 2 - save config")
                .Print(" i - instances menu")
                .Print(" t - triggers menu")
                .Print(" q - quit (and start slipstream)")
                .Spacer();

            var input = tui.ReadKey();

            switch (input)
            {
                case '1':
                    LoadConfig(tui);
                    break;

                case '2':
                    SaveConfig(tui);
                    break;

                case 'i':
                case 'I':
                    _instanceMenuHandler.Show(tui.NewScope("instances"));
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