namespace Slipstream.Domain.Entities;

public interface IInstance : IEntity
{
    IInstanceConfiguration? Configuration { get; }

    Task MainAsync(CancellationToken cancel);
}
