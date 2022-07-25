namespace Slipstream.Domain.DynamicProperties;

public interface IPropertyVisitor
{
    void VisitStringProperty(StringProperty property);
    void VisitUnsupportedProperty(UnsupportedProperty property);
}