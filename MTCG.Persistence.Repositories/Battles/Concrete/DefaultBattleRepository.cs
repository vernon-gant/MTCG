using Microsoft.Extensions.Logging;

using MTCG.Domain;
using MTCG.Persistence.Database;

namespace MTCG.Persistence.Repositories.Battles.Concrete;

public class DefaultBattleRepository : AbstractRepository, BattleRepository
{
    private readonly ILogger<DefaultBattleRepository> _logger;

    public DefaultBattleRepository(DatabaseConfig databaseConfig, ILogger<DefaultBattleRepository> logger) : base(databaseConfig)
    {
        _logger = logger;
    }

    public DefaultBattleRepository(DatabaseConfig databaseConfig) : base(databaseConfig) { }

    public async Task SaveBattleResult(BattleResult battleResult)
    {
        return;
    }

}