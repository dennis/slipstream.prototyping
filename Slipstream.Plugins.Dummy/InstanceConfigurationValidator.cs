using FluentValidation;

namespace Slipstream.Plugins.Dummy;

internal class InstanceConfigurationValidator : AbstractValidator<InstanceConfiguration>
{
    public InstanceConfigurationValidator()
    {
        RuleFor(x => x.String).NotEmpty();
    }
}
