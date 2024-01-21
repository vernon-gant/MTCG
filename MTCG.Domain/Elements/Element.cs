using MTCG.Domain.Cards;

namespace MTCG.Domain.Elements;

public class Element
{

    public int ElementId { get; set; }

    public Card? Card { get; set; }

    public string Name { get; set; } = string.Empty;

    public ElementAbility Ability { get; protected set; } = null!;

    public Element() { }

    public Element(Element element)
    {
        ElementId = element.ElementId;
        Card = element.Card;
        Name = element.Name;
        Ability = element.Ability;
    }

    public static Element FromElement(Element element)
    {
        return new Element
        {
            ElementId = element.ElementId,
            Card = element.Card,
            Name = element.Name,
        };
    }

}