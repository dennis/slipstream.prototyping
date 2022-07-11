namespace Slipstream.Domain.Forms;

public class FormCollection
{
    public IEnumerable<IFormElement> Elements { get; }

    public FormCollection(IEnumerable<IFormElement> elements)
        => Elements = elements;

    public void Visit(IFormCollectionVisitor visitor)
    {
        foreach (var element in Elements)
        {
            element.Accept(visitor);
        }
    }

    public void Populate(object config)
    {
        Visit(new PopulateConfig(config));
    }
}