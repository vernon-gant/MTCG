﻿using System.Collections.Concurrent;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MTCG.Domain;

namespace MTCG.Services.BattleServices.Battle;

public class BattleResultsStorage
{
    private readonly ConcurrentDictionary<BattleRequest, TaskCompletionSource<BattleResult>> _battleResults
        = new();

    private readonly ILogger<BattleResultsStorage> _logger;

    public BattleResultsStorage(ILogger<BattleResultsStorage> logger)
    {
        _logger = logger;
    }

    public void StoreBattleResult(BattleRequest userRequest, BattleRequest opponentRequest, BattleResult result)
    {
        TaskCompletionSource<BattleResult> completionSource = new();
        completionSource.SetResult(result);
        _battleResults.AddOrUpdate(userRequest, completionSource, (key, oldSource) =>
        {
            oldSource.SetResult(result);
            return oldSource;
        });
        _battleResults.AddOrUpdate(opponentRequest, completionSource, (key, oldSource) =>
        {
            oldSource.SetResult(result);
            return oldSource;
        });

        _logger.LogInformation($"Stored battle result for {userRequest.GetHashCode()} and {opponentRequest.GetHashCode()}");
    }

    public async ValueTask<BattleResult> GetBattleResultAsync(BattleRequest request)
    {
        TaskCompletionSource<BattleResult> completionSource = _battleResults.GetOrAdd(request, _ => new TaskCompletionSource<BattleResult>());
        return await completionSource.Task;;
    }
}