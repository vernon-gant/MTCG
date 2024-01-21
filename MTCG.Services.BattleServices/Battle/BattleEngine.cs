using MTCG.Domain;

namespace MTCG.Services.BattleServices.Battle;

public interface BattleEngine
{

    ValueTask<BattleResult> BattleAsync(BattleRequest battleRequest);

}