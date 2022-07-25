namespace Slipstream.Domain.DynamicProperties;

public interface IProperty
{
    string Name { get; }

    void Accept(IPropertyVisitor visitor);
}
