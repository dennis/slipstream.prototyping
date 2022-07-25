using Slipstream.Domain.DynamicProperties;

namespace Slipstream.CLI;

internal class ConsoleFormVisitor : IPropertyVisitor
{
    private readonly TUIHelper _tui;

    public ConsoleFormVisitor(TUIHelper tuiHelper)
    {
        _tui = tuiHelper;
    }

    public void VisitStringProperty(StringProperty element)
    {
        element.Value = _tui.Prompt(element.Name, element.Value, element.FormHelp);
    }

    public void VisitUnsupportedProperty(UnsupportedProperty element)
    {
        _tui.Debug(element.Name + " not supported. Ignored");
    }
}