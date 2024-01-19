using MTCG.Domain;

namespace MTCG.Persistence.Repositories.Cards;

public interface CardsRepository
{

    ValueTask<List<Card>> GetUserCardsAsync(int userId);

    ValueTask<List<Element>> GetCardElementsAsync();

}