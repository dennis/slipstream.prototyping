using Slipstream.Domain.Entities;

namespace Slipstream.Domain.Rules;

public interface IRuleFactory : IEntityFactory<IRule, IRuleConfiguration>
{

}