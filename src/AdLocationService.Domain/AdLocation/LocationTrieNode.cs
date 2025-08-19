using System.Collections;
using System.Collections.Concurrent;

namespace AdLocationService.Domain.AdLocation;

public class LocationTrieNode<T> : IEnumerable<KeyValuePair<string, HashSet<T>>>
{
    public Dictionary<string, LocationTrieNode<T>> Children { get; } = [];
    public HashSet<T> Values { get; } = [];


    public void Insert(string[] parts, T value, int index = 0)
    {
        if (index >= parts.Length)
        {
            Values.Add(value);
            return;
        }

        var part = parts[index];
        if (!Children.ContainsKey(part))
            Children[part] = new LocationTrieNode<T>();

        Children[part].Insert(parts, value, index + 1);
    }

    public void Search(string[] parts, HashSet<T> result, int index = 0)
    {
        result.UnionWith(Values);

        if (index >= parts.Length)
            return;

        if (Children.TryGetValue(parts[index], out var child))
            child.Search(parts, result, index + 1);
    }

    public IEnumerator<KeyValuePair<string, HashSet<T>>> GetEnumerator() =>
        LocationTrieNode<T>.Traverse("", this).GetEnumerator();

    private static IEnumerable<KeyValuePair<string, HashSet<T>>> Traverse(
        string prefix,
        LocationTrieNode<T> node
    )
    {
        if (node.Values.Count > 0)
            yield return new KeyValuePair<string, HashSet<T>>(prefix, node.Values);

        foreach (var kv in node.Children)
        {
            var path = string.IsNullOrEmpty(prefix) ? "/" + kv.Key : $"{prefix}/{kv.Key}";
            foreach (var child in LocationTrieNode<T>.Traverse(path, kv.Value))
                yield return child;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
