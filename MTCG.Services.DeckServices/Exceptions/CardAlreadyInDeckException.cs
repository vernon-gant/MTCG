namespace MTCG.Services.DeckServices.Exceptions;

public class CardAlreadyInDeckException : Exception
{

    public List<Guid> CardIds { get; }

    public CardAlreadyInDeckException(List<Guid> cardIds)
    {
        CardIds = cardIds;
    }

}