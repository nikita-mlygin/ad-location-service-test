namespace AdLocationService.Domain.AdLocation.Construct;

public interface ILocationTrieBuilder
{
    void BuildTrieFromStream(Stream stream, LocationTrieNode<string> root);
}
