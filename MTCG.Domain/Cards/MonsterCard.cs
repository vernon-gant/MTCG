namespace MTCG.Domain.Cards;

public class MonsterCard : Card
{

    private SpecialMonsterAbility? _specialMonsterAbility;

    public MonsterCard() { }

    public MonsterCard(Card card) : base(card) { }

}