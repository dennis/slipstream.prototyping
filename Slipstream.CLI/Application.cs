using Slipstream.Domain;
using Slipstream.Domain.Forms;

namespace Slipstream.CLI;

internal class Application
{
    private readonly IRegistry _registry;
    private readonly TUIHelper _tui;
    private readonly ConsoleFormVisitor _consoleFormVisitor;
    private readonly IFormGenerator _formGenerator;

    public Application(IRegistry registry, IFormGenerator formGenerator, TUIHelper tuiHelper, ConsoleFormVisitor consoleFormVisitor)
    {
        _registry = registry;
        _tui = tuiHelper;
        _consoleFormVisitor = consoleFormVisitor;
        _formGenerator = formGenerator;
    }

    public async Task RunAsync()
    {
        _tui
            .PrintHeading("Slipstream")
            .Spacer();

        MainMenu();

        Console.WriteLine("Starting Slipstream");

        _registry.Start();

        await Task.Delay(-1).ConfigureAwait(false);

        _registry.Stop();

        Console.WriteLine("Slipstream stopped");
    }

    private void MainMenu()
    {
        bool quit = false;
        do
        {
            _tui.PrintHeading("Main menu:")
                .Print(" 1 - list plugins")
                .Print(" 2 - create instance")
                .Print(" q - quit (and start slipstream)")
                .Spacer();

            var input = Console.ReadKey();

            _tui.Spacer().Spacer();

            switch (input.KeyChar)
            {
                case '1':
                    ShowPlugins();
                    break;

                case '2':
                    CreateInstance();
                    break;

                case 'q':
                case 'Q':
                    quit = true;
                    break;

                default:
                    _tui.Error("Unknown item");
                    break;
            }        

            _tui.Reset();
        } while (!quit);
    }

    private void CreateInstance()
    {
        ShowPlugins();
        var pluginName = _tui.Prompt("plugin name");
        var plugin = _registry.GetPlugin(pluginName);

        if (plugin is null)
        {
            _tui.Error("Unknown plugin: " + pluginName);
            return;
        }

        _tui.Spacer();

        var config = plugin.CreateInstanceConfiguration();
        var form = _formGenerator.Generate(config);

        var instanceName = _tui.Prompt("instance name");

        _tui.PrintStrong("instance configuration");

        form.Visit(_consoleFormVisitor);
        form.Populate(config);

        var result = plugin.ValidateInstanceConfiguration(config);

        if (!result.Errors.Any())
        {
            _registry.CreateInstance(plugin, instanceName, config);
        }
        else
        {
            foreach (var err in result.Errors)
            {
                _tui.Error($"{err.Key}: {err.Value}");
            }
        }
    }

    private void ShowPlugins()
    {
        _tui.PrintStrong("Plugins available:");

        foreach (var plugin in _registry.Plugins)
        {
            _tui.Print($" - {plugin.Name}");

            foreach (var instanceName in plugin.InstanceNames)
            {
                _tui.Print($"   - {instanceName}");
            }
        }

        _tui.Spacer();
    }
}