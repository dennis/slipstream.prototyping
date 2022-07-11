using Slipstream.Domain.ValueObjects;

namespace Slipstream.Domain.Entities;

public interface IInstanceFactory
{
    IInstance Create<TConfig>(EntityName name, TConfig config);
}
