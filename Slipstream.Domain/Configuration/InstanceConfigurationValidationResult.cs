using FluentValidation.Results;

namespace Slipstream.Domain.Configuration;

public class InstanceConfigurationValidationResult
{
    private readonly Dictionary<string, string> _errors = new();

    public IDictionary<string, string> Errors { get => _errors; }

    public void AddError(string propertyName, string errorMessage)
    {
        _errors.Add(propertyName, errorMessage);
    }

    public bool IsValid()
        => _errors.Keys.Count == 0;

    public static InstanceConfigurationValidationResult FromFluentValidationResult(ValidationResult input)
    {
        var output = new InstanceConfigurationValidationResult();

        foreach (var error in input.Errors)
        {
            output.AddError(error.PropertyName, error.ErrorMessage);
        }

        return output;
    }
}
