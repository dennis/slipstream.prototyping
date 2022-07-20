using Slipstream.CLI.MenuHandlers;
using Slipstream.Domain;
using Slipstream.Domain.Rules;
using Slipstream.Plugins.Dummy;

namespace Slipstream.CLI;

internal class Application
{
    private readonly IRegistry _registry;
    private readonly TUIHelper _tui;
    private readonly MainMenuHandler _mainMenuHandler;

    public Application(
        IRegistry registry, 
        TUIHelper tuiHelper,
        MainMenuHandler mainMenuHandler
    )
    {
        _registry = registry;
        _tui = tuiHelper;
        _mainMenuHandler = mainMenuHandler;
    }

    public async Task RunAsync()
    {
        _tui
            .PrintHeading("Slipstream")
            .Spacer();

        _mainMenuHandler.Show(_tui);

        _tui.PrintStrong("Starting Slipstream");

        _registry.Start();

        await Task.Delay(-1).ConfigureAwait(false);

        _registry.Stop();

        _tui.PrintStrong("Slipstream stopped");
    }
}