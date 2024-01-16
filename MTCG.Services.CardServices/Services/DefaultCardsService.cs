using MTCG.Persistance.Repositories;

namespace MTCG.Services.Cards.cards;

public class DefaultCardsService : CardsService
{

    private readonly CardsRepository _cardsRepository;

    public DefaultCardsService(CardsRepository cardsRepository)
    {
        _cardsRepository = cardsRepository;
    }

}