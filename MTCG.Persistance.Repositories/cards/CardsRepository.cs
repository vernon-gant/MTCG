using MTCG.Domain;

namespace MTCG.Persistance.Repositories;

public interface CardsRepository
{
    ValueTask<IEnumerable<CardDto>> GetAllCards();

    ValueTask<IEnumerable<Card>> GetUserCards(string username);

}