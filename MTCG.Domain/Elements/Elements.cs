using MTCG.Domain.Cards;

namespace MTCG.Domain.Elements;

public class Fire : Element
{

    public Fire()
    {
        Ability = new FireAbility();
    }

    public Fire(Element element) : base(element)
    {
        Ability = new FireAbility();
    }

}

public class Water : Element
{

    public Water()
    {
        Ability = new WaterAbility();
    }

    public Water(Element element) : base(element)
    {
        Ability = new WaterAbility();
    }

}

public class Earth : Element
{

    public Earth()
    {
        Ability = new EarthAbility();
    }

    public Earth(Element element) : base(element)
    {
        Ability = new EarthAbility();
    }

}

public class Air : Element
{

    public Air()
    {
        Ability = new AirAbility();
    }

    public Air(Element element) : base(element)
    {
        Ability = new AirAbility();
    }

}

public class Shadow : Element
{

    public Shadow()
    {
        Ability = new ShadowAbility();
    }

    public Shadow(Element element) : base(element)
    {
        Ability = new ShadowAbility();
    }

}

public class Normal : Element
{

    public Normal()
    {
        Ability = new NormalAbility();
    }

    public Normal(Element element) : base(element)
    {
        Ability = new NormalAbility();
    }

}