using MTCG.Domain.Cards;
using MTCG.Domain.Elements;

namespace MTCG.Persistence.Repositories.Cards;

public interface CardRepository
{

    ValueTask<List<Card>> GetUserCardsAsync(int userId);

    ValueTask<List<Element>> GetElementsAsync();

}