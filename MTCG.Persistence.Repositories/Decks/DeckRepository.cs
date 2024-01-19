using MTCG.Domain;

namespace MTCG.Persistence.Repositories.Decks;

public interface DeckRepository
{

    ValueTask<Deck?> GetDeckByIdAsync(int deckId);

    ValueTask<List<Deck>> GetUserDecksAsync(int userId);

    ValueTask<Deck?> GetUserActiveDeckAsync(int userId);

    ValueTask<Deck> AddUserDeckAsync(Deck deckToAdd);

    Task SetUserActiveDeckAsync(int deckId);

    Task UnsetUserActiveDeckAsync(int userId);

}