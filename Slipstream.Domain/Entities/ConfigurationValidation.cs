using FluentValidation.Results;

namespace Slipstream.Domain.Entities;

public class ConfigurationValidation
{
    private readonly Dictionary<string, string> _errors = new();

    public IDictionary<string, string> Errors { get => _errors; }
    public static ConfigurationValidation OK => new();

    public void AddError(string propertyName, string errorMessage)
    {
        _errors.Add(propertyName, errorMessage);
    }

    public bool IsValid()
        => _errors.Keys.Count == 0;

    public static ConfigurationValidation FromFluentValidationResult(ValidationResult input)
    {
        var output = new ConfigurationValidation();

        foreach (var error in input.Errors)
        {
            output.AddError(error.PropertyName, error.ErrorMessage);
        }

        return output;
    }
}
