using FluentValidation.Results;

using System.Reactive.Linq;
namespace Slipstream.Core.Configuration;

public class ConfigurationValidationResult
{
    private readonly Dictionary<string, string> _errors = new();

    public IDictionary<string, string> Errors { get => _errors; }

    public void AddError(string propertyName, string errorMessage)
    {
        _errors.Add(propertyName, errorMessage);
    }

    public bool IsValid()
        => _errors.Keys.Count == 0;

    public static ConfigurationValidationResult FromFluentValidationResult(ValidationResult input)
    {
        var output = new ConfigurationValidationResult();

        foreach (var error in input.Errors)
        {
            output.AddError(error.PropertyName, error.ErrorMessage);
        }

        return output;
    }
}
