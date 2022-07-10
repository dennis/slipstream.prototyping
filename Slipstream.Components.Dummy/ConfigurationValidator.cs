using FluentValidation;

namespace Slipstream.Components.Dummy;

internal class ConfigurationValidator : AbstractValidator<Configuration>
{
    public ConfigurationValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
