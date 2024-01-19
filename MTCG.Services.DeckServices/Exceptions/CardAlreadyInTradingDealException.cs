namespace MTCG.Services.DeckServices.Exceptions;

public class CardAlreadyInTradingDealException : Exception
{

    public List<Guid> CardIds { get; }

    public CardAlreadyInTradingDealException(List<Guid> cardIds)
    {
        CardIds = cardIds;
    }

}