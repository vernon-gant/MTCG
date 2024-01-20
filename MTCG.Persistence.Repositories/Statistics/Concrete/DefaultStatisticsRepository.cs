using System.Data.Common;

using Microsoft.Extensions.Logging;

using MTCG.Domain;
using MTCG.Persistance.Database;
using MTCG.Persistence.Database;

namespace MTCG.Persistence.Repositories.Statistics.Concrete;

public class DefaultStatisticsRepository : AbstractRepository, StatisticsRepository
{

    private ILogger<DefaultStatisticsRepository> _logger;

    public DefaultStatisticsRepository(DatabaseConfig databaseConfig, ILogger<DefaultStatisticsRepository> logger) : base(databaseConfig)
    {
        _logger = logger;
    }

    public async ValueTask<UserStatistics> GetUserStatisticsAsync(int userId)
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();

            UserStatistics? userStatistics = await connection.QuerySingleAsync<UserStatistics>(
                "SELECT " +
                "u.username AS UserName, " +
                "u.elo AS ELO, " +
                "(SELECT COUNT(*) FROM battles b WHERE (b.result = 'PlayerOneWin' AND b.playeroneid = @UserId) OR (b.result = 'PlayerTwoWin' AND b.playertwoid = @UserId)) AS Wins, " +
                "(SELECT COUNT(*) FROM battles b WHERE (b.result = 'PlayerOneWin' AND b.playertwoid = @UserId) OR (b.result = 'PlayerTwoWin' AND b.playeroneid = @UserId)) AS Losses, " +
                "(SELECT SUM(CASE WHEN b.playeroneid = @UserId AND b.playeroneelochange > 0 THEN b.playeroneelochange ELSE 0 END) + SUM(CASE WHEN b.playertwoid = @UserId AND b.playertwoelochange > 0 THEN b.playertwoelochange ELSE 0 END) FROM battles b) AS PointsWon, " +
                "(SELECT SUM(CASE WHEN b.playeroneid = @UserId AND b.playeroneelochange < 0 THEN -b.playeroneelochange ELSE 0 END) + SUM(CASE WHEN b.playertwoid = @UserId AND b.playertwoelochange < 0 THEN -b.playertwoelochange ELSE 0 END) FROM battles b) AS PointsLost, " +
                "(SELECT COUNT(*) FROM TradingDeals WHERE RespondingUserId = @UserId OR OfferingUserId = @UserId) AS CardsTraded " +
                "FROM users u " +
                "WHERE u.userid = @UserId",
                new { UserId = userId }
            );

            return userStatistics!;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting user statistics!");

            throw;
        }
    }

    public async ValueTask<List<UserStatistics>> GetScoreboardAsync()
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();

            IEnumerable<UserStatistics> userStatistics = await connection.QueryAsync<UserStatistics>(
                "SELECT " +
                "u.username AS UserName, " +
                "u.elo AS ELO, " +
                "(SELECT COUNT(*) FROM battles b WHERE (b.result = 'PlayerOneWin' AND b.playeroneid = u.userid) OR (b.result = 'PlayerTwoWin' AND b.playertwoid = u.userid)) AS Wins, " +
                "(SELECT COUNT(*) FROM battles b WHERE (b.result = 'PlayerOneWin' AND b.playertwoid = u.userid) OR (b.result = 'PlayerTwoWin' AND b.playeroneid = u.userid)) AS Losses, " +
                "(SELECT SUM(CASE WHEN b.playeroneid = u.userid AND b.playeroneelochange > 0 THEN b.playeroneelochange ELSE 0 END) + SUM(CASE WHEN b.playertwoid = u.userid AND b.playertwoelochange > 0 THEN b.playertwoelochange ELSE 0 END) FROM battles b) AS PointsWon, " +
                "(SELECT SUM(CASE WHEN b.playeroneid = u.userid AND b.playeroneelochange < 0 THEN -b.playeroneelochange ELSE 0 END) + SUM(CASE WHEN b.playertwoid = u.userid AND b.playertwoelochange < 0 THEN -b.playertwoelochange ELSE 0 END) FROM battles b) AS PointsLost, " +
                "(SELECT COUNT(*) FROM TradingDeals WHERE RespondingUserId = u.userid OR OfferingUserId = u.userid) AS CardsTraded " +
                "FROM users u " +
                "WHERE u.userid IN (SELECT userid FROM users WHERE isadmin = false) " +
                "ORDER BY u.elo DESC"
            );

            return userStatistics.ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting user statistics!");

            throw;
        }
    }

}