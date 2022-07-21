using Slipstream.Domain.Rules;

namespace Slipstream.Domain;

public interface IRuleContainer
{
    public List<IRule> Rules { get; }
}