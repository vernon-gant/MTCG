using MTCG.Domain.Elements;

namespace MTCG.Domain.Cards;

public class Card
{

    public int CardId { get; set; }

    public Guid UserCardId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Damage { get; set; }

    public int ElementId { get; set; }

    public Element? Element { get; set; }

    public CardAbility Ability { get; set; } = null!;

    public List<string> Events { get; } = new ();


    public Card() { }

    public Card(Card card)
    {
        CardId = card.CardId;
        UserCardId = card.UserCardId;
        Name = card.Name;
        Damage = card.Damage;
        ElementId = card.ElementId;
        Element = card.Element;
        Ability = card.Ability;
    }

    public static Card ForBattle(Card card)
    {
        return new Card
        {
            CardId = card.CardId,
            UserCardId = card.UserCardId,
            Name = card.Name,
            Damage = card.Damage,
            ElementId = card.ElementId,
            Element = card.Element,
            Ability = card.Ability,
        };
    }

    public static bool operator <(Card lhs, Card rhs)
    {
        return lhs.Damage < rhs.Damage;
    }

    public static bool operator >(Card lhs, Card rhs)
    {
        return lhs.Damage > rhs.Damage;
    }

}