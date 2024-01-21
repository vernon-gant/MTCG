namespace MTCG.Services.BattleServices.ViewModels;

public class BattleResultViewModel
{

    public int BattleId { get; set; }

    public string EnemyUserName { get; set; } = string.Empty;

    public string Result { get; set; } = string.Empty;

    public int ELOChange { get; set; }

    public List<BattleEventViewModel> BattleEvents { get; set; } = new ();

}