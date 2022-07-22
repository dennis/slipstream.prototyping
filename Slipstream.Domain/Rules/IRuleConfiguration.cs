using Slipstream.Domain.Entities;

namespace Slipstream.Domain.Rules;

public interface IRuleConfiguration : IEntityConfiguration
{
    string Trigger { get; set; }
    string Action { get; set; }
}
