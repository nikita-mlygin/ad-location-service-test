using AdLocationService.Domain.AdLocation.Construct;
using AdLocationService.Domain.AdLocation.Repository;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AdLocationService.Api.AdLocation.Upload;

public class UploadTrieEndpoint(ITrieRepository repo, ILocationTrieBuilder builder)
    : EndpointWithoutRequest
{
    private readonly ITrieRepository repo = repo;
    private readonly ILocationTrieBuilder builder = builder;

    public override void Configure()
    {
        Post("/trie/upload-file");
        AllowAnonymous();
        AllowFormData();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var form = await HttpContext.Request.ReadFormAsync(ct);
        var files = form.Files;

        if (files.Count == 0)
        {
            await Send.ResultAsync(TypedResults.BadRequest("No file uploaded"));
            return;
        }

        var file = files[0];

        using var stream = file.OpenReadStream();

        try
        {
            builder.BuildTrieFromStream(stream, repo.Root);
        }
        catch (FormatException)
        {
            await Send.ResultAsync(TypedResults.BadRequest("Invalid file format"));
            return;
        }

        await Send.OkAsync(cancellation: ct);
    }
}
