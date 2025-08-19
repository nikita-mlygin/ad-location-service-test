namespace AdLocationService.Domain.AdLocation.Repository;

public class TrieInMemoryRepository : ITrieRepository
{
    public LocationTrieNode<string> Root { get; } = new LocationTrieNode<string>();
}
