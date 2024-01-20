using MTCG.Domain.Cards;

namespace MTCG.Services.Cards.Services;

public interface CardMapperService
{

    ValueTask<List<Card>> MapCardsAsync(List<Card> cards);

}