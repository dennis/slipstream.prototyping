using System.Reflection;

namespace Slipstream.Domain.Forms;

public class UnsupportedFormElement : IFormElement
{
    public UnsupportedFormElement(PropertyInfo _)
    {
    }

    public string Name { get; init; } = "";

    public void Accept(IFormCollectionVisitor visitor)
    {
        visitor.VisitUnsupportedFormElement(this);
    }
}
