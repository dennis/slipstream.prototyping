using Slipstream.Core.ValueObjects;

namespace Slipstream.Core.Entities;

public interface IInstanceFactory
{
    IInstance Create<TConfig>(EntityName name, TConfig config);
}
