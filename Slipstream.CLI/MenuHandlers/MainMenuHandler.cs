using Slipstream.Domain;
using Slipstream.Domain.Rules;

namespace Slipstream.CLI.MenuHandlers;

internal class MainMenuHandler
{
    private readonly IRegistry _registry;
    private readonly IApplicationSettings _applicationSettings;
    private readonly TriggerMenuHandler _triggerMenuHandler;
    private readonly InstanceMenuHandler _instanceMenuHandler;
    private readonly RuleMenuHandler _ruleMenuHandler;
    private readonly ActionMenuHandler _actionMenuHandler;
    private readonly IRuleFactory _ruleFactory;

    public MainMenuHandler(
        IRegistry registry,
        IApplicationSettings applicationSettings,
        TriggerMenuHandler triggerMenuHandler,
        InstanceMenuHandler instanceMenuHandler,
        RuleMenuHandler ruleMenuHandler,
        IRuleFactory ruleFactory,
        ActionMenuHandler actionMenuHandler)
    {
        _registry = registry;
        _applicationSettings = applicationSettings;
        _triggerMenuHandler = triggerMenuHandler;
        _instanceMenuHandler = instanceMenuHandler;
        _ruleMenuHandler = ruleMenuHandler;
        _ruleFactory = ruleFactory;
        _actionMenuHandler = actionMenuHandler;
    }

    public void Show(TUIHelper tui)
    {
        bool quit = false;
        do
        {
            tui.PrintHeading("Main menu:")
                .Print(" 1 - load config")
                .Print(" 2 - save config")
                .Print(" a - actions menu")
                .Print(" i - instances menu")
                .Print(" r - rules menu")
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

                case 'a':
                case 'A':
                    _actionMenuHandler.Show(tui.NewScope("actions"));
                    break;

                case 'i':
                case 'I':
                    _instanceMenuHandler.Show(tui.NewScope("instances"));
                    break;

                case 'r':
                case 'R':
                    _ruleMenuHandler.Show(tui.NewScope("rules"));
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
            var factory = _registry.InstanceContainer[instanceTypeName];

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
            var factory = _registry.TriggerContainer.Types[triggerTypeName];

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

        tui.PrintStrong("Loading rules");

        foreach (var (_, ruleName) in _applicationSettings.ReadRules())
        {
            var config = _ruleFactory.ConfigurationJsonDecoder(_applicationSettings.LoadRule(ruleName));
            var rule = _ruleFactory.Create(ruleName, config);

            _registry.AddRule(rule);

            tui.Print($" - {ruleName}");
        }

        tui.PrintStrong("Loading actions");

        foreach (var (entityTypeName, entityName) in _applicationSettings.ReadActions())
        {
            var factory = _registry.ActionContainer.Types[entityTypeName];

            var config = factory.ConfigurationJsonDecoder(_applicationSettings.LoadAction(entityTypeName, entityName));
            if (config is not null && !factory.Validate(config).IsValid())
            {
                tui.Error($" - {entityTypeName} / {entityName} - configuration contains error. Ignoring");
                continue;
            }

            var action = factory.Create(entityName, config);

            _registry.AddAction(action);

            tui.Print($" - {entityTypeName} / {entityName}");
        }

        tui.Print("Done");
    }

    private void SaveConfig(TUIHelper tui)
    {
        tui.PrintStrong("Saving...");

        foreach (var instance in _registry.InstanceContainer.Instances)
        {
            _applicationSettings.SaveInstance(instance);
        }

        foreach (var trigger in _registry.TriggerContainer.Triggers)
        {
            _applicationSettings.SaveTrigger(trigger);
        }

        foreach (var rule in _registry.RuleContainer.Rules)
        {
            _applicationSettings.SaveRule(rule);
        }

        foreach (var action in _registry.ActionContainer.Actions)
        {
            _applicationSettings.SaveAction(action);
        }

        tui.Print("Done");
    }
}