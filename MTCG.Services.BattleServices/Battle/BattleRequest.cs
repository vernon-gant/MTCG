using MTCG.Domain;

namespace MTCG.Services.BattleServices.Battle;

public class BattleRequest
{

    public required User User { get; set; }

    public required Deck Deck { get; set; }

}