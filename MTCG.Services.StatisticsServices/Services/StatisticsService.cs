using MTCG.Services.StatisticsServices.ViewModels;

namespace MTCG.Services.StatisticsServices.Services;

public interface StatisticsService
{

    ValueTask<UserStatisticsViewModel> GetUserStatisticsAsync(string userName);

    ValueTask<List<UserStatisticsViewModel>> GetScoreboardAsync();

}