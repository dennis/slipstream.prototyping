using FluentValidation;

namespace Slipstream.Plugins.Dummy;

public class KeyPressTriggerConfigurationValidator : AbstractValidator<KeyPressTriggerConfiguration>
{
    public KeyPressTriggerConfigurationValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty()
            .MaximumLength(1);
    }
}
