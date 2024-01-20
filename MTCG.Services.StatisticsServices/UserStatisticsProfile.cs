using AutoMapper;

using MTCG.Domain;
using MTCG.Services.StatisticsServices.ViewModels;

namespace MTCG.Services.StatisticsServices;

public class UserStatisticsProfile : Profile
{

    public UserStatisticsProfile()
    {
        CreateMap<UserStatistics, UserStatisticsViewModel>();
    }

}