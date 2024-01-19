namespace MTCG.Services.DeckServices.Exceptions;

public class CardNotInUserStackException : Exception
{

    public List<Guid> CardIds { get; }

    public CardNotInUserStackException(List<Guid> cardIds)
    {
        CardIds = cardIds;
    }

}