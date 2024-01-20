using MTCG.Domain.Elements;

namespace MTCG.Domain.Cards;

public class Card
{

    public Card() { }

    public Card(Card card)
    {
        CardId = card.CardId;
        UserCardId = card.UserCardId;
        Name = card.Name;
        Damage = card.Damage;
        ElementId = card.ElementId;
        Element = card.Element;
    }

    public int CardId { get; set; }

    public Guid UserCardId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Damage { get; set; }

    public int ElementId { get; set; }

    public Element? Element { get; set; }

}