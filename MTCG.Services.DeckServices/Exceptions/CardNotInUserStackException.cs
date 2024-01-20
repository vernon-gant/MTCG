namespace MTCG.Services.DeckServices.Exceptions;

public class CardNotInUserStackException : Exception
{

    public CardNotInUserStackException(List<Guid> cardIds)
    {
        CardIds = cardIds;
    }

    public List<Guid> CardIds { get; }

}