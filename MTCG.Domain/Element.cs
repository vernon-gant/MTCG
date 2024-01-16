namespace MTCG.Domain;

public class Element
{

    private Card _card;

    private readonly SpecialElementAbility _specialElementAbility;

    public Element(SpecialElementAbility specialElementAbility, Card card)
    {
        _specialElementAbility = specialElementAbility;
        _card = card;
    }

}