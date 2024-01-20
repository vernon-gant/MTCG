using MTCG.Domain.Cards;

namespace MTCG.Domain;

public class Deck
{

    public int DeckId { get; set; }

    public int UserId { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public List<Card> Cards { get; set; } = new ();

}