using MTCG.Domain;

namespace MTCG.Persistence.Repositories.Statistics;

public interface StatisticsRepository
{

    ValueTask<UserStatistics> GetUserStatisticsAsync(int userId);

    ValueTask<List<UserStatistics>> GetScoreboardAsync();

}