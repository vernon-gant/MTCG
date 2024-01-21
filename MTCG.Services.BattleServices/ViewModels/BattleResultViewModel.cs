using MTCG.Domain;

namespace MTCG.Services.BattleServices.ViewModels;

public class BattleResultViewModel
{
    public string EnemyUserName { get; set; } = string.Empty;

    public string Result { get; set; } = string.Empty;

    public int ELOChange { get; set; }

    public List<BattleEvent> BattleEvents { get; set; } = new ();

}