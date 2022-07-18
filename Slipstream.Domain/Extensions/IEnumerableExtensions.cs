namespace Slipstream.Domain.Extensions;

public static class IEnumerableExtensions
{
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> src)
        => src.Select((item, index) => (item, index));
}
