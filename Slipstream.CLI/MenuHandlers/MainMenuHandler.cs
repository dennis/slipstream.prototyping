using Slipstream.Domain;

namespace Slipstream.CLI.MenuHandlers;

internal class MainMenuHandler
{
    private readonly IApplicationSettings _applicationSettings;
    private readonly TriggerMenuHandler _triggerMenuHandler;
    private readonly InstanceMenuHandler _instanceMenuHandler;
    private readonly RuleMenuHandler _ruleMenuHandler;
    private readonly ActionMenuHandler _actionMenuHandler;

    public MainMenuHandler(
        IApplicationSettings applicationSettings,
        TriggerMenuHandler triggerMenuHandler,
        InstanceMenuHandler instanceMenuHandler,
        RuleMenuHandler ruleMenuHandler,
        ActionMenuHandler actionMenuHandler)
    {
        _applicationSettings = applicationSettings;
        _triggerMenuHandler = triggerMenuHandler;
        _instanceMenuHandler = instanceMenuHandler;
        _ruleMenuHandler = ruleMenuHandler;
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
        _applicationSettings.Load(msg => tui.Print(msg), err => tui.Error(err));
    }

    private void SaveConfig(TUIHelper tui)
    {
        _applicationSettings.Save(msg => tui.Print(msg));
    }
}