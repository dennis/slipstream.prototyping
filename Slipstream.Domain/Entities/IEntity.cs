using Slipstream.Domain.ValueObjects;

namespace Slipstream.Domain.Entities;

public interface IEntity
{
    EntityName Name { get; }
}
