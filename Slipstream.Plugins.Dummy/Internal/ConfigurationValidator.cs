using FluentValidation;

namespace Slipstream.Plugins.Dummy.Internal;

internal class ConfigurationValidator : AbstractValidator<Configuration>
{
    public ConfigurationValidator()
    {
        RuleFor(x => x.String).NotEmpty();
    }
}
