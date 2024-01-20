using AutoMapper;

using MTCG.Domain;
using MTCG.Persistence.Repositories.Statistics;
using MTCG.Persistence.Repositories.Users;
using MTCG.Services.StatisticsServices.ViewModels;
using MTCG.Services.UserService;

namespace MTCG.Services.StatisticsServices.Services.Concrete;

public class DefaultStatisticsService : StatisticsService
{

    private readonly UserRepository _userRepository;

    private readonly StatisticsRepository _statisticsRepository;

    private readonly IMapper _mapper;

    public DefaultStatisticsService(UserRepository userRepository, StatisticsRepository statisticsRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _statisticsRepository = statisticsRepository;
        _mapper = mapper;
    }

    public async ValueTask<UserStatisticsViewModel> GetUserStatisticsAsync(string userName)
    {
        User? user = await _userRepository.GetByUserName(userName);

        if (user == null) throw new UserNotFoundException();

        UserStatistics userStatistics = await _statisticsRepository.GetUserStatisticsAsync(user.UserId);

        return _mapper.Map<UserStatisticsViewModel>(userStatistics);
    }

    public async ValueTask<List<UserStatisticsViewModel>> GetScoreboardAsync()
    {
        List<UserStatistics> userStatistics = await _statisticsRepository.GetScoreboardAsync();

        return _mapper.Map<List<UserStatisticsViewModel>>(userStatistics);
    }

}