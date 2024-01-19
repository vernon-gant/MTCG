namespace MTCG.Domain;

public class Card
{

    public int CardId { get; set; }

    public Guid UserCardId { get; set; }

    public string Name { get; set; }

    public int Damage { get; set; }

    public int ElementId { get; set; }

    public Element? Element { get; set; }

}