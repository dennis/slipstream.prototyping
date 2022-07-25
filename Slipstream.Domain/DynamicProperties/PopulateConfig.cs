namespace Slipstream.Domain.DynamicProperties;

public class PopulateConfigVisitor : IPropertyVisitor
{
    private readonly object _obj;

    public PopulateConfigVisitor(object obj)
    {
        _obj = obj;
    }

    public void VisitStringProperty(StringProperty property)
    {
        property.Property.SetValue(_obj, property.Value);
    }

    public void VisitUnsupportedProperty(UnsupportedProperty property)
    {
    }
}