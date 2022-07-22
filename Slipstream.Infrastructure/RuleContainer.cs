using Slipstream.Domain;
using Slipstream.Domain.Rules;

namespace Slipstream.Infrastructure;

internal class RuleContainer : IRuleContainer
{
    public List<IRule> Rules { get; } = new();
}
