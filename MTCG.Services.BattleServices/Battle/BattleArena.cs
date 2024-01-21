using MTCG.Domain;

namespace MTCG.Services.BattleServices.Battle;

public class BattleArena
{

    private readonly BattleResultsStorage _battleResultsStorage;

    public BattleArena(BattleResultsStorage battleResultsStorage)
    {
        _battleResultsStorage = battleResultsStorage;
    }

    public async Task BattleAsync(BattleRequest battleRequest, BattleRequest opponentRequest)
    {
        BattleResult battleResult = await PerformBattleAsync(battleRequest, opponentRequest);
        _battleResultsStorage.StoreBattleResult(battleRequest, opponentRequest, battleResult);
    }

    private async Task<BattleResult> PerformBattleAsync(BattleRequest battleRequest, BattleRequest opponentRequest)
    {
        await Task.Delay(1000);

        return new BattleResult();
    }

}