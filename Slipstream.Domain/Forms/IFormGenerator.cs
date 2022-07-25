namespace Slipstream.Domain.Forms;

public interface IFormGenerator
{
    public FormCollection Generate(object o);
}
