using Slipstream.Domain.Entities;

namespace Slipstream.Domain.Instances;

public interface IInstance : IEntity
{
    IInstanceConfiguration? Configuration { get; }

    Task MainAsync(CancellationToken cancel);
}
