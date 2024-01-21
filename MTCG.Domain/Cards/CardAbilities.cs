namespace MTCG.Domain.Cards;

public interface CardAbility
{

    void Apply(Card thisCard, Card enemyCard);

}

public abstract class MonsterCardAbility : CardAbility
{

    protected abstract void ApplySpecial(Card thisCard, Card enemyCard);

    public void Apply(Card thisCard, Card enemyCard)
    {
        if (!(thisCard is MonsterCard && enemyCard is MonsterCard)) thisCard!.Element.Ability.Apply(thisCard.Element, enemyCard!.Element);

        ApplySpecial(thisCard, enemyCard);
    }

}

public class GoblinAbility : MonsterCardAbility
{

    protected override void ApplySpecial(Card thisCard, Card enemyCard)
    {
        if (enemyCard is Dragon)
        {
            thisCard.Events.Add("Goblin is too afraid of Dragon to attack.");
            thisCard.Damage = 0;
        }
    }

}

public class DragonAbility : MonsterCardAbility
{

    protected override void ApplySpecial(Card thisCard, Card enemyCard)
    {
        if (enemyCard is Wizard)
        {
            thisCard.Events.Add("Dragon cannot harm the Wizard due to magical control.");
            thisCard.Damage = 0;
        }
    }

}

public class WizardAbility : MonsterCardAbility
{

    protected override void ApplySpecial(Card thisCard, Card enemyCard)
    {
        if (enemyCard is Ork)
        {
            thisCard.Events.Add("Wizard controls the Ork, making it harmless.");
            thisCard.Damage = 0;
        }
    }

}

public class OrkAbility : MonsterCardAbility
{

    protected override void ApplySpecial(Card thisCard, Card enemyCard)
    {
        if (enemyCard is Knight)
        {
            thisCard.Events.Add("Ork's attack has no effect on the heavily armored Knight.");
            thisCard.Damage = 0;
        }
    }

}

public class KnightAbility : MonsterCardAbility
{

    protected override void ApplySpecial(Card thisCard, Card enemyCard)
    {
        if (enemyCard is Goblin)
        {
            thisCard.Events.Add("Knight effortlessly defeats the Goblin.");
            thisCard.Damage = 0;
        }
    }
}

public class KrakenAbility : MonsterCardAbility
{

    protected override void ApplySpecial(Card thisCard, Card enemyCard)
    {
        if (enemyCard is SpellCard)
        {
            thisCard.Events.Add("Kraken is immune to spells, negating any damage.");
            enemyCard.Damage = 0;
        }
    }

}

public class FireElfAbility : MonsterCardAbility
{

    protected override void ApplySpecial(Card thisCard, Card enemyCard)
    {
        if (enemyCard is Dragon)
        {
            thisCard.Events.Add("FireElf evades the Dragon's attack with ease.");
            thisCard.Damage = 0;
        }
    }

}

public abstract class SpellCardAbility : CardAbility
{

    protected abstract void ApplySpecial(Card thisCard, Card enemyCard);

    public void Apply(Card thisCard, Card enemyCard)
    {
        thisCard!.Element.Ability.Apply(thisCard.Element, enemyCard!.Element);

        ApplySpecial(thisCard, enemyCard);
    }

}

public class FlameStrikeAbility : SpellCardAbility
{

    protected override void ApplySpecial(Card thisCard, Card enemyCard)
    {
        thisCard.Damage *= 2;
        thisCard.Events.Add("FlameStrike's damage is doubled against its target.");
    }

}

public class WaterBlastAbility : SpellCardAbility
{

    protected override void ApplySpecial(Card thisCard, Card enemyCard)
    {
        enemyCard.Damage /= 2;
        thisCard.Events.Add("WaterBlast halves the damage of its target.");
    }

}

public class EarthShatterAbility : SpellCardAbility
{

    protected override void ApplySpecial(Card thisCard, Card enemyCard)
    {
        enemyCard.Damage = 0;
        thisCard.Damage += 10;
        thisCard.Events.Add("EarthShatter nullifies the target's damage and increases its own.");
    }

}

public class AirSliceAbility : SpellCardAbility
{

    protected override void ApplySpecial(Card thisCard, Card enemyCard)
    {
        enemyCard.Damage /= 4;
        thisCard.Events.Add("AirSlice significantly reduces the target's damage.");
    }

}

public class ShadowFogAbility : SpellCardAbility
{

    protected override void ApplySpecial(Card thisCard, Card enemyCard)
    {
        thisCard.Damage *= 2;
        enemyCard.Damage /= 2;
        thisCard.Events.Add("ShadowFog doubles its damage and halves that of its target.");
    }

}

public class HealAbility : SpellCardAbility
{

    protected override void ApplySpecial(Card thisCard, Card enemyCard)
    {
        thisCard.Damage *= 2;
        thisCard.Events.Add("Heal doubles its damage, reinforcing its strength.");
    }

}

