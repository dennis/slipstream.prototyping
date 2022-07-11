using System.Reflection;

namespace Slipstream.Domain.Forms;

public class StringFormElement : IFormElement
{
    public string Name { get; private set; }
    public string? Value { get; set; }
    public PropertyInfo Property { get; }

    public StringFormElement(string name, string? value, PropertyInfo property)
    {
        Name = name;
        Value = value;
        Property = property;
    }

    public void Accept(IFormCollectionVisitor visitor)
    {
        visitor.VisitStringFormElement(this);
    }
}
