namespace Slipstream.Domain.Rules;

public class RuleConfiguration : IRuleConfiguration
{
    public string Trigger { get; set; } = "";
    public string Action { get; set; } = "";
}