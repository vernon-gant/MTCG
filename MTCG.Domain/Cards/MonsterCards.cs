namespace MTCG.Domain.Cards;

public class MonsterCard : Card
{

    public MonsterCard() { }

    public MonsterCard(Card card) : base(card) { }

}

public class Goblin : MonsterCard
{

    public Goblin()
    {
        Ability = new GoblinAbility();
    }

    public Goblin(Card card) : base(card)
    {
        Ability = new GoblinAbility();
    }

}

public class Dragon : MonsterCard
{

    public Dragon()
    {
        Ability = new DragonAbility();
    }

    public Dragon(Card card) : base(card)
    {
        Ability = new DragonAbility();
    }

}

public class Wizard : MonsterCard
{

    public Wizard()
    {
        Ability = new WizardAbility();
    }

    public Wizard(Card card) : base(card)
    {
        Ability = new WizardAbility();
    }

}

public class Ork : MonsterCard
{

    public Ork()
    {
        Ability = new OrkAbility();
    }

    public Ork(Card card) : base(card)
    {
        Ability = new OrkAbility();
    }

}

public class Knight : MonsterCard
{

    public Knight()
    {
        Ability = new KnightAbility();
    }

    public Knight(Card card) : base(card)
    {
        Ability = new KnightAbility();
    }

}

public class Kraken : MonsterCard
{

    public Kraken()
    {
        Ability = new KrakenAbility();
    }

    public Kraken(Card card) : base(card)
    {
        Ability = new KrakenAbility();
    }

}

public class FireElf : MonsterCard
{

    public FireElf()
    {
        Ability = new FireElfAbility();
    }

    public FireElf(Card card) : base(card)
    {
        Ability = new FireElfAbility();
    }

}