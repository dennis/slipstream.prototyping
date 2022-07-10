
using System.Text.RegularExpressions;

namespace Slipstream.Core.ValueObjects;

public class EntityName
{
    private readonly string _value;

    private EntityName(string value)
    {
        _value = value;
    }

    public static EntityName From(string value)
    {
        value = value.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"ComponentName '{value}' is not valid");
        }

        if (!Regex.IsMatch(value, @"^[a-z0-9_-]+$"))
        {
            throw new ArgumentException($"ComponentName '{value}' contains invalid characters (a-z0-9, _ and - allowed)");
        }

        return new EntityName(value);
    }

    public static implicit operator string(EntityName value)
        => value._value;

    public static implicit operator EntityName(string value)
        => From(value);

    public override string ToString()
        => _value;

    public override int GetHashCode()
        => _value.GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var typed = (EntityName)obj;
        return _value.Equals(typed._value);
    }
}
