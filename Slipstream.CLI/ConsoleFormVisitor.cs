using Slipstream.Domain.Attributes;
using Slipstream.Domain.Forms;

namespace Slipstream.CLI;

internal class ConsoleFormVisitor : IFormCollectionVisitor
{
    private readonly TUIHelper _tui;

    public ConsoleFormVisitor(TUIHelper tuiHelper)
    {
        _tui = tuiHelper;
    }

    public void VisitStringFormElement(StringFormElement element)
    {
        element.Value = _tui.Prompt(element.Name, element.Value, element.FormHelp);
    }

    public void VisitUnsupportedFormElement(UnsupportedFormElement element)
    {
    }
}