namespace Slipstream.Domain.DynamicProperties;

public class PropertyCollection
{
    public IEnumerable<IProperty> Elements { get; }

    public PropertyCollection(IEnumerable<IProperty> elements)
        => Elements = elements;

    public void Visit(IPropertyVisitor visitor)
    {
        foreach (var element in Elements)
        {
            element.Accept(visitor);
        }
    }

    public void Populate(object config)
    {
        Visit(new PopulateConfigVisitor(config));
    }
}