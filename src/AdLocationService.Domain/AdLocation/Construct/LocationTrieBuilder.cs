using System.Text.RegularExpressions;

namespace AdLocationService.Domain.AdLocation.Construct;

public partial class LocationTrieBuilder : ILocationTrieBuilder
{
    [GeneratedRegex(@"^(?<company>[^:]+):(?<locations>.+)$")]
    private static partial Regex LineRegex();

    public void BuildTrieFromStream(Stream stream, LocationTrieNode<string> root)
    {
        using var reader = new StreamReader(stream);

        string? line;

        while ((line = reader.ReadLine()) != null)
        {
            ProcessLine(line, root);
        }
    }

    private static void ProcessLine(string line, LocationTrieNode<string> root)
    {
        var trimmed = line.Trim();
        if (string.IsNullOrEmpty(trimmed))
            return;

        var match = LineRegex().Match(trimmed);
        if (!match.Success)
            throw new FormatException($"Неверный формат строки: {line}");

        var company = match.Groups["company"].Value.Trim();
        var locations = match
            .Groups["locations"]
            .Value.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrEmpty(l));

        foreach (var loc in locations)
        {
            if (!loc.StartsWith('/'))
                throw new FormatException($"Локация должна начинаться с '/': {loc}");

            root.Insert(loc.Split('/', StringSplitOptions.RemoveEmptyEntries), company);
        }
    }
}
