using MTCG.Domain.Elements;

namespace MTCG.Domain.Cards;

public abstract class ElementAbility
{
    public abstract void Apply(Element thisElement, Element enemyElement);
}

public class FireAbility : ElementAbility
{
    public override void Apply(Element thisElement, Element enemyElement)
    {
        if (thisElement is Water && enemyElement is Fire)
        {
            thisElement.Card.Events.Add($"{thisElement.Card.Name} quenches {enemyElement.Card.Name}'s flames! Damage is doubled.");
            thisElement.Card.Damage *= 2;
        }
        else if (thisElement is Fire && enemyElement is Water)
        {
            thisElement.Card.Events.Add($"{thisElement.Card.Name} is weakened by {enemyElement.Card.Name}'s water! Damage is halved.");
            thisElement.Card.Damage /= 2;
        }
    }
}

public class WaterAbility : ElementAbility
{
    public override void Apply(Element thisElement, Element enemyElement)
    {
        if (thisElement is Water && enemyElement is Normal)
        {
            thisElement.Card.Events.Add($"{thisElement.Card.Name} engulfs {enemyElement.Card.Name} with a torrent! Damage is doubled.");
            thisElement.Card.Damage *= 2;
        }
        else if (thisElement is Normal && enemyElement is Water)
        {
            thisElement.Card.Events.Add($"{thisElement.Card.Name} is unphased by {enemyElement.Card.Name}'s water. Damage remains the same.");
        }
    }
}

public class EarthAbility : ElementAbility
{
    public override void Apply(Element thisElement, Element enemyElement)
    {
        if (thisElement is Fire && enemyElement is Earth)
        {
            thisElement.Card.Events.Add($"{thisElement.Card.Name} scorches {enemyElement.Card.Name}! Damage is doubled.");
            thisElement.Card.Damage *= 2;
        }
        else if (thisElement is Earth && enemyElement is Fire)
        {
            thisElement.Card.Events.Add($"{thisElement.Card.Name} smothers {enemyElement.Card.Name}'s flames. Damage is halved.");
            thisElement.Card.Damage /= 2;
        }
    }
}

public class AirAbility : ElementAbility
{
    public override void Apply(Element thisElement, Element enemyElement)
    {
        if (thisElement is Air && enemyElement is Earth)
        {
            thisElement.Card.Events.Add($"{thisElement.Card.Name} creates a whirlwind around {enemyElement.Card.Name}! Damage is doubled.");
            thisElement.Card.Damage *= 2;
        }
        else if (thisElement is Earth && enemyElement is Air)
        {
            thisElement.Card.Events.Add($"{thisElement.Card.Name} is grounded and unaffected by {enemyElement.Card.Name}'s air. Damage remains the same.");
        }
    }
}

public class ShadowAbility : ElementAbility
{
    public override void Apply(Element thisElement, Element enemyElement)
    {
        if (thisElement is Shadow && enemyElement is Normal)
        {
            thisElement.Card.Events.Add($"{thisElement.Card.Name} envelops {enemyElement.Card.Name} in darkness! Damage is doubled.");
            thisElement.Card.Damage *= 2;
        }
        else if (thisElement is Normal && enemyElement is Shadow)
        {
            thisElement.Card.Events.Add($"{thisElement.Card.Name} shines light on {enemyElement.Card.Name}'s shadow. Damage is halved.");
            thisElement.Card.Damage /= 2;
        }
    }
}

public class NormalAbility : ElementAbility
{
    public override void Apply(Element thisElement, Element enemyElement)
    {
        thisElement.Card.Events.Add($"{thisElement.Card.Name} is unaffected by {enemyElement.Card.Name}. Damage remains the same.");
    }
}
