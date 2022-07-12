using FluentValidation;

namespace Slipstream.Plugins.Dummy.Internal;

internal class ConfigurationValidator : AbstractValidator<InstanceConfiguration>
{
    public ConfigurationValidator()
    {
        RuleFor(x => x.String).NotEmpty();
    }
}
