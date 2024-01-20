namespace MTCG.Services.DeckServices.Exceptions;

public class CardAlreadyInDeckException : Exception
{

    public CardAlreadyInDeckException(List<Guid> cardIds)
    {
        CardIds = cardIds;
    }

    public List<Guid> CardIds { get; }

}