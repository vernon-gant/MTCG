using MTCG.Domain;

namespace MTCG.Persistence.Repositories.Decks;

public interface DeckRepository
{

    ValueTask<Deck?> GetByIdAsync(int deckId);

    ValueTask<List<Deck>> GetUserDecksAsync(int userId);

    ValueTask<Deck?> GetUserActiveDeckAsync(int userId);

    ValueTask<Deck> AddDeckAsync(Deck deckToAdd);

    Task SetActiveDeckAsync(int deckId);

    Task UnsetActiveDeckAsync(int userId);

}