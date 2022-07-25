using System.Reflection;

namespace Slipstream.Domain.DynamicProperties;

public class UnsupportedProperty : IProperty
{
    public UnsupportedProperty(PropertyInfo _)
    {
    }

    public string Name { get; init; } = "";

    public void Accept(IPropertyVisitor visitor)
    {
        visitor.VisitUnsupportedProperty(this);
    }
}
