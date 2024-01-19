namespace MTCG.Domain;

public class Element
{

    public int ElementId { get; set; }

    private Card? _card;

    public required string Name { get; set; }

    private readonly SpecialElementAbility? _specialElementAbility;

}