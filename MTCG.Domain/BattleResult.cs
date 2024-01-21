using MTCG.Domain.Cards;

namespace MTCG.Domain;

public class BattleResult
{

    public int BattleId { get; set; }

    public User PlayerOne { get; set; } = new ();

    public List<Card> PlayerOneCards { get; set; } = new ();

    public User PlayerTwo { get; set; } = new ();

    public List<Card> PlayerTwoCards { get; set; } = new ();

    public string Result { get; set; } = string.Empty;

    public int PlayerOneELOChange { get; set; }

    public int PlayerTwoELOChange { get; set; }

    public List<BattleEvent> BattleEvents { get; set; } = new ();

}