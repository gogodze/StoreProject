using Domain.Abstractions;

namespace Domain.Common;

public static class ICollectionExt
{
    public static ICollection<T> AddRange<T>(this ICollection<T> source, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            source.Add(item);
        }
        return source;
    }
}