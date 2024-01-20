using MTCG.Domain.Cards;

namespace MTCG.Domain.Elements;

public class Element
{

    private SpecialElementAbility? _specialElementAbility;

    public Element() { }

    public Element(Element element)
    {
        ElementId = element.ElementId;
        Card = element.Card;
        Name = element.Name;
        _specialElementAbility = element._specialElementAbility;
    }

    public int ElementId { get; set; }

    public Card? Card { get; set; }

    public string Name { get; set; } = string.Empty;

    public static Element FromElement(Element element)
    {
        return new Element
        {
            ElementId = element.ElementId,
            Card = element.Card,
            Name = element.Name,
            _specialElementAbility = element._specialElementAbility
        };
    }

}