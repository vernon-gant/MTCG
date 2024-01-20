using AutoMapper;

using MTCG.Domain.Cards;
using MTCG.Services.UserService.ViewModels;

namespace MTCG.Services.UserService;

public class CardProfile : Profile
{

    public CardProfile()
    {
        CreateMap<Card, UserCardViewModel>();
    }

}