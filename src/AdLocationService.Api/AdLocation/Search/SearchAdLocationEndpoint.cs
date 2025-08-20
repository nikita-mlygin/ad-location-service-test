using AdLocationService.Domain.AdLocation;
using AdLocationService.Domain.AdLocation.Repository;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace AdLocationService.Api.AdLocation.Search;

public class SearchTrieRequest
{
    [QueryParam]
    public string Path { get; set; } = "";
}

public class SearchTrieResponse
{
    public string Path { get; set; } = "";
    public List<string> Platforms { get; set; } = new();
}

public class SearchTrieEndpoint(ITrieRepository repo)
    : Endpoint<SearchTrieRequest, SearchTrieResponse>
{
    private readonly ITrieRepository repo = repo;

    public override void Configure()
    {
        Get("/trie/search");
        AllowAnonymous();
        Description(b => b.Accepts<SearchTrieRequest>());
    }

    public override async Task HandleAsync(SearchTrieRequest req, CancellationToken ct)
    {
        var result = new HashSet<string>();
        repo.Root.Search(req.Path.Split('/', StringSplitOptions.RemoveEmptyEntries), result);
        await Send.OkAsync(new SearchTrieResponse { Path = req.Path, Platforms = [.. result] }, ct);
    }
}
