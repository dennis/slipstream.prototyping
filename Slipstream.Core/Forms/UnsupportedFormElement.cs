using System.Reflection;

namespace Slipstream.Core.Forms;

public class UnsupportedFormElement : IFormElement
{
    private PropertyInfo _property;

    public UnsupportedFormElement(PropertyInfo property)
    {
        _property = property;
    }

    public string Name { get; init; } = "";

    public void Accept(IFormCollectionVisitor visitor)
    {
        visitor.VisitUnsupportedFormElement(this);
    }
}
