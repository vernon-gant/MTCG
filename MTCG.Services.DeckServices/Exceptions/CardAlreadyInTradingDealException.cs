namespace MTCG.Services.DeckServices.Exceptions;

public class CardAlreadyInTradingDealException : Exception
{

    public CardAlreadyInTradingDealException(List<Guid> cardIds)
    {
        CardIds = cardIds;
    }

    public List<Guid> CardIds { get; }

}