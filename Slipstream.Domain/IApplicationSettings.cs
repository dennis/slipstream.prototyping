namespace Slipstream.Domain;

public interface IApplicationSettings
{
    void Load(Action<string> print, Action<string> error);
    void Save(Action<string> print);
}
