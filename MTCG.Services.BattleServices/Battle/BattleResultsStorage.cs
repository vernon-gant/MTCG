using System.Collections.Concurrent;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MTCG.Domain;

namespace MTCG.Services.BattleServices.Battle;

public class BattleResultsStorage
{

    private readonly ConcurrentDictionary<BattleRequest, TaskCompletionSource<BattleResult>> _battleResults
        = new ();

    private readonly ILogger<BattleResultsStorage> _logger;

    public BattleResultsStorage(ILogger<BattleResultsStorage> logger)
    {
        _logger = logger;
    }

    public void StoreBattleResult(BattleRequest userRequest, BattleRequest opponentRequest, BattleResult result)
    {
        _logger.LogInformation($"Storing battle result for {userRequest.GetHashCode()} and {opponentRequest.GetHashCode()}");
        TaskCompletionSource<BattleResult> completionSource = new ();
        completionSource.SetResult(result);

        _battleResults.TryAdd(userRequest, completionSource);

        _battleResults.TryGetValue(opponentRequest, out TaskCompletionSource<BattleResult>? opponentCompletionSource);
        opponentCompletionSource?.SetResult(result);

        _logger.LogInformation($"Stored battle result for {userRequest.GetHashCode()} and {opponentRequest.GetHashCode()}");
    }

    public async ValueTask<BattleResult> GetBattleResultAsync(BattleRequest request)
    {
        _logger.LogInformation($"Waiting for battle result for {request.GetHashCode()} with user {request.User.UserId}");
        TaskCompletionSource<BattleResult> completionSource = _battleResults.GetOrAdd(request, _ => new TaskCompletionSource<BattleResult>());
        BattleResult readyResult = await completionSource.Task;
        _logger.LogInformation($"Got battle result for {request.GetHashCode()} with user {request.User.UserId}");
        _battleResults.Remove(request, out _);

        return readyResult;
    }

}