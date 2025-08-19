namespace AdLocationService.Domain.AdLocation.Repository;

public interface ITrieRepository
{
    LocationTrieNode<string> Root { get; }
}
