namespace MTCG.Domain.Cards;

public class SpellCard : Card
{

    protected SpellCard() { }

    protected SpellCard(Card card) : base(card) { }

}

public class FlameStrike : SpellCard
{

    public FlameStrike()
    {
        Ability = new FlameStrikeAbility();
    }

    public FlameStrike(Card card) : base(card)
    {
        Ability = new FlameStrikeAbility();
    }

}

public class WaterBlast : SpellCard
{

    public WaterBlast()
    {
        Ability = new WaterBlastAbility();
    }

    public WaterBlast(Card card) : base(card)
    {
        Ability = new WaterBlastAbility();
    }

}

public class EarthShatter : SpellCard
{

    public EarthShatter()
    {
        Ability = new EarthShatterAbility();
    }

    public EarthShatter(Card card) : base(card)
    {
        Ability = new EarthShatterAbility();
    }

}

public class AirSlice : SpellCard
{

    public AirSlice()
    {
        Ability = new AirSliceAbility();
    }

    public AirSlice(Card card) : base(card)
    {
        Ability = new AirSliceAbility();
    }

}

public class ShadowFog : SpellCard
{

    public ShadowFog()
    {
        Ability = new ShadowFogAbility();
    }

    public ShadowFog(Card card) : base(card)
    {
        Ability = new ShadowFogAbility();
    }

}

public class Heal : SpellCard
{

    public Heal()
    {
        Ability = new HealAbility();
    }

    public Heal(Card card) : base(card)
    {
        Ability = new HealAbility();
    }

}