namespace MTCG.Domain;

public class BattleEvent
{

    public int Round { get; set; }

    public string FirstCardName { get; set; } = string.Empty;

    public List<string> FirstCardEvents { get; set; } = new ();

    public int FirstCardFinalDamage { get; set; }

    public string SecondCardName { get; set; } = string.Empty;

    public List<string> SecondCardEvents { get; set; } = new ();

    public int SecondCardFinalDamage { get; set; }

    public string WinnerCardName { get; set; } = string.Empty;

}