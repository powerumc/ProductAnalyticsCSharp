namespace ProductAnalytics.Extensions;

internal static class DictionaryExtensions
{
    /// <summary>
    ///     Two dictionary merge and return new dictionary.
    /// </summary>
    /// <param name="left">Dictionary</param>
    /// <param name="right">Dictionary</param>
    /// <returns>new dictionary object</returns>
    /// <exception cref="ArgumentNullException">If argument is null</exception>
    internal static IDictionary<string, object?> Merge(this IDictionary<string, object?> left,
        IDictionary<string, object?> right)
    {
        _ = left ?? throw new ArgumentNullException(nameof(left));
        _ = right ?? throw new ArgumentNullException(nameof(right));

        return left
            .Union(right)
            .GroupBy(o => o.Key)
            .Select(o => o.FirstOrDefault(item => item.Value != null))
            .ToDictionary(o => o.Key, o => o.Value);
    }
}