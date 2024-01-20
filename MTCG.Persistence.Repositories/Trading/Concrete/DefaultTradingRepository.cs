using System.Data.Common;

using Microsoft.Extensions.Logging;

using MTCG.Domain;
using MTCG.Persistance.Database;
using MTCG.Persistence.Database;

namespace MTCG.Persistence.Repositories.Trading.Concrete;

public class DefaultTradingRepository : AbstractRepository, TradingRepository
{

    private readonly ILogger<DefaultTradingRepository> _logger;

    public DefaultTradingRepository(DatabaseConfig databaseConfig, ILogger<DefaultTradingRepository> logger) : base(databaseConfig)
    {
        _logger = logger;
    }

    public async ValueTask<TradingDeal?> GetByIdAsync(Guid tradingDealId)
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();

            TradingDeal? tradingDeal = await connection.QuerySingleAsync<TradingDeal>("SELECT * FROM tradingdeals WHERE tradingdealid = @tradingdealid",
                                                                                      new { tradingdealid = tradingDealId });

            return tradingDeal;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting trading deal by id.");

            throw;
        }
    }

    public async ValueTask<List<TradingDeal>> GetAvailableAsync()
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();

            List<TradingDeal> tradingDeals = (await connection.QueryAsync<TradingDeal>("SELECT * FROM tradingdeals WHERE respondinguserid IS NULL")).ToList();

            List<User> users = (await connection.QueryAsync<User>("SELECT * FROM users WHERE userid IN (SELECT offeringuserid FROM tradingdeals WHERE respondinguserid IS NULL)"))
                .ToList();

            List<Card> cards =
                (await connection.QueryAsync<Card>(
                    "SELECT * FROM usercards JOIN cards on cards.cardid = usercards.cardid WHERE usercards.usercardid IN (SELECT offeringusercardid FROM tradingdeals WHERE respondinguserid IS NULL)"))
                .ToList();

            foreach (TradingDeal tradingDeal in tradingDeals)
            {
                tradingDeal.OfferingUser = users.First(user => user.UserId == tradingDeal.OfferingUserId);
                tradingDeal.OfferingUserCard = cards.First(card => card.UserCardId == tradingDeal.OfferingUserCardId);
            }

            return tradingDeals;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting available trading deals.");

            throw;
        }
    }

    public async Task CreateAsync(TradingDeal tradingDeal)
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await using DbCommand command = connection.CreateCommand();
            await connection.OpenAsync();

            command.CommandText =
                "INSERT INTO tradingdeals (tradingdealid, offeringuserid, offeringusercardid, requiredcardtype, requiredminimumdamage) VALUES (@tradingdealid, @offeringuserid, @offeringusercardid, @requiredcardtype, @requiredminimumdamage)";

            command.AddParameters(new
            {
                tradingdealid = tradingDeal.TradingDealId, offeringuserid = tradingDeal.OfferingUserId, offeringusercardid = tradingDeal.OfferingUserCardId,
                requiredcardtype = tradingDeal.RequiredCardType, requiredminimumdamage = tradingDeal.RequiredMinimumDamage
            });
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating trading deal.");

            throw;
        }
    }

    public async Task CarryOutAsync(TradingDeal tradingDeal)
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await using DbCommand command = connection.CreateCommand();
            await connection.OpenAsync();
            await using DbTransaction transaction = await connection.BeginTransactionAsync();

            command.CommandText = "UPDATE tradingdeals SET respondinguserid = @respondinguserid, respondingusercardid = @respondingusercardid WHERE tradingdealid = @tradingdealid";

            command.AddParameters(new
            {
                tradingdealid = tradingDeal.TradingDealId, respondinguserid = tradingDeal.RespondingUserId, respondingusercardid = tradingDeal.RespondingUserCardId
            });
            await command.ExecuteNonQueryAsync();

            command.Parameters.Clear();
            command.CommandText = "UPDATE usercards SET userid = @respondinguserid WHERE usercardid = @offeringusercardid";
            command.AddParameters(new { offeringusercardid = tradingDeal.OfferingUserCardId, respondinguserid = tradingDeal.RespondingUserId });
            await command.ExecuteNonQueryAsync();

            command.Parameters.Clear();
            command.CommandText = "UPDATE usercards SET userid = @offeringuserid WHERE usercardid = @respondingusercardid";
            command.AddParameters(new { offeringuserid = tradingDeal.OfferingUserId, respondingusercardid = tradingDeal.RespondingUserCardId });
            await command.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while carrying out trading deal.");

            throw;
        }
    }

    public async Task DeleteAsync(Guid tradingDealId)
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await using DbCommand command = connection.CreateCommand();
            await connection.OpenAsync();

            command.CommandText = "DELETE FROM tradingdeals WHERE tradingdealid = @tradingdealid";
            command.AddParameters(new { tradingdealid = tradingDealId });
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting trading deal.");

            throw;
        }
    }

}