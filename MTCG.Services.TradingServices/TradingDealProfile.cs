using AutoMapper;

using MTCG.Domain;
using MTCG.Services.TradingServices.DTO;
using MTCG.Services.TradingServices.ViewModels;

namespace MTCG.Services.TradingServices;

public class TradingDealProfile : Profile
{

    public TradingDealProfile()
    {
        CreateMap<TradingDealCreationDTO, TradingDeal>();

        CreateMap<TradingDeal, TradingDealViewModel>()
            .ForMember(dest => dest.OfferingUserName, opt => opt.MapFrom(src => src.OfferingUser != null ? src.OfferingUser.UserName : string.Empty))
            .ForMember(dest => dest.OfferingCardName, opt => opt.MapFrom(src => src.OfferingUserCard != null ? src.OfferingUserCard.Name : string.Empty))
            .ForMember(dest => dest.OfferingCardDamage, opt => opt.MapFrom(src => src.OfferingUserCard != null ? src.OfferingUserCard.Damage : 0));
    }

}