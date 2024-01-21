using System.Data.Common;

using Microsoft.Extensions.Logging;

using MTCG.Domain;
using MTCG.Domain.Cards;
using MTCG.Persistance.Database;
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
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();
            await using DbTransaction transaction = await connection.BeginTransactionAsync();

            await using DbCommand command = connection.CreateCommand();

            command.CommandText =
                "INSERT INTO battles (playeroneid, playertwoid, result, playeroneelochange, playertwoelochange) VALUES (@playeroneid, @playertwoid, CAST(@result AS battle_result), @playeroneelochange, @playertwoelochange) RETURNING battleid";

            command.AddParameters(new
            {
                playeroneid = battleResult.PlayerOne.UserId, playertwoid = battleResult.PlayerTwo.UserId, result = battleResult.Result,
                playeroneelochange = battleResult.PlayerOneELOChange, playertwoelochange = battleResult.PlayerTwoELOChange
            });

            battleResult.BattleId = await command.ExecuteScalarAsync() as int? ?? throw new Exception("BattleId was null");


            await SaveBattleDeck(battleResult.PlayerOneCards, battleResult.BattleId, battleResult.PlayerOne.UserId, connection);
            await SaveBattleDeck(battleResult.PlayerTwoCards, battleResult.BattleId, battleResult.PlayerTwo.UserId, connection);

            await transaction.CommitAsync();
        }
        catch (Exception e)

        {
            _logger.LogError(e, "Error while saving battle result");

            throw;
        }
    }

    private async Task SaveBattleDeck(List<Card> cards, int battleId, int userId, DbConnection connection)
    {
        try
        {
            await using DbCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO battledecks (battleid, userid) VALUES (@battleid, @userid) RETURNING battledeckid";
            command.AddParameters(new { battleid = battleId, userid = userId });

            int battleDeckId = await command.ExecuteScalarAsync() as int? ?? throw new Exception("BattleDeckId was null");

            command.CommandText = "INSERT INTO battledeckcontents (battledeckid, cardid,damage) VALUES (@battledeckid, @cardid, @damage)";

            foreach (Card card in cards)
            {
                command.AddParameters(new { battledeckid = battleDeckId, cardid = card.CardId, damage = card.Damage });
                await command.ExecuteNonQueryAsync();
                command.Parameters.Clear();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while saving battle deck");

            throw;
        }
    }

}