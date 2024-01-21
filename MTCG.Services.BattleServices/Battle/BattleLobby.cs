using MTCG.Services.BattleServices.Battle;

public class BattleLobby
{

    private BattleRequest? _waitingBattleRequest;

    private ManualResetEventSlim _battleReadyEvent = new (false);

    public BattleRequest? GetEnemy(BattleRequest battleRequest)
    {
        lock (this)
        {
            if (_waitingBattleRequest == null)
            {
                _waitingBattleRequest = battleRequest;

                return null;
            }

            BattleRequest waitingRequest = _waitingBattleRequest;

            _waitingBattleRequest = null;

            _battleReadyEvent.Set();

            return waitingRequest;
        }
    }

    public void WaitForOpponent()
    {
        _battleReadyEvent.Wait();
    }

}