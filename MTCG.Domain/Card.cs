namespace MTCG.Domain;

public enum Type
{
    Monster,
    Spell
}

public class Card
{

    public string Name { get; set; }

    public int Damage { get; set; }

    public Enum Type { get; set; }

    public Element Element { get; set; }

}