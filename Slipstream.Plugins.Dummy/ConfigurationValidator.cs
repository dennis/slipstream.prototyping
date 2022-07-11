using FluentValidation;

namespace Slipstream.Plugins.Dummy;

internal class ConfigurationValidator : AbstractValidator<Configuration>
{
    public ConfigurationValidator()
    {
        RuleFor(x => x.String).NotEmpty();
    }
}
