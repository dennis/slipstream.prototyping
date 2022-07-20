using System.Text.RegularExpressions;

namespace Slipstream.Domain.ValueObjects;

public class EntityTypeName
{
    private readonly string _value;

    private EntityTypeName(string value)
    {
        _value = value;
    }

    public static EntityTypeName From(string value)
    {
        value = value.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"EntityTypeName '{value}' is not valid");
        }

        if (!Regex.IsMatch(value, @"^[a-z0-9_-]+$"))
        {
            throw new ArgumentException($"EntityTypeName '{value}' contains invalid characters (a-z0-9, _ and - allowed)");
        }

        return new EntityTypeName(value);
    }

    public static implicit operator string(EntityTypeName value)
        => value._value;

    public static implicit operator EntityTypeName(string value)
        => From(value);

    public override string ToString()
        => _value;

    public override int GetHashCode()
        => _value.GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var typed = (EntityTypeName)obj;
        return _value.Equals(typed._value);
    }
}
