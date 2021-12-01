namespace AdventOfCode.Common;

public static class ListExtensions
{
    public static int GetSequenceHashCode<T>(this IEnumerable<T> sequence)
    {
        var hash = new HashCode();
        foreach (var element in sequence)
        {
            hash.Add(element);
        }

        return hash.ToHashCode();
    }
}