using AutoMapper;

using MTCG.Domain;
using MTCG.Services.BattleServices.ViewModels;

namespace MTCG.Services.BattleServices;

public class BattleResultProfile : Profile
{

    public BattleResultProfile()
    {
        CreateMap<BattleEvent, BattleEventViewModel>();
        CreateMap<BattleResult, BattleResultViewModel>();
    }

}