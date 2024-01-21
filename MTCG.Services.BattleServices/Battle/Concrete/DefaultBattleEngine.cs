using MTCG.Domain;
using MTCG.Persistence.Repositories.Battles;

namespace MTCG.Services.BattleServices.Battle.Concrete;

public class DefaultBattleEngine : BattleEngine
{

    private readonly BattleRepository _battleRepository;

    private readonly BattleLobby _battleLobby;

    private readonly BattleArena _battleArena;

    private readonly BattleResultsStorage _battleResultsStorage;

    public DefaultBattleEngine(BattleLobby battleLobby, BattleArena battleArena, BattleResultsStorage battleResultsStorage, BattleRepository battleRepository)
    {
        _battleLobby = battleLobby;
        _battleArena = battleArena;
        _battleResultsStorage = battleResultsStorage;
        _battleRepository = battleRepository;
    }

    public async ValueTask<BattleResult> BattleAsync(BattleRequest battleRequest)
    {
        BattleRequest? opponentRequest = _battleLobby.GetEnemy(battleRequest);

        if (opponentRequest is null)
        {
            _battleLobby.WaitForOpponent();

            return await _battleResultsStorage.GetBattleResultAsync(battleRequest);
        }

        await _battleArena.BattleAsync(battleRequest, opponentRequest);
        BattleResult battleResult = await _battleResultsStorage.GetBattleResultAsync(battleRequest);
        await _battleRepository.SaveBattleResult(battleResult);

        return battleResult;
    }

}