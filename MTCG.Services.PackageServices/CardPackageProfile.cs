using AutoMapper;

using MTCG.Domain;
using MTCG.Services.PackageServices.Dto;
using MTCG.Services.PackageServices.ViewModels;

namespace MTCG.Services.PackageServices;

public class CardPackageProfile : Profile
{

    public CardPackageProfile()
    {
        CreateMap<CardPackageCreationDto, CardPackage>()
            .ForMember(dest => dest.Cards, opt => opt.MapFrom(src => src.Cards.Select(card => new Card
            {
                UserCardId = card.UserCardId,
                Name = card.Name,
                Damage = card.Damage
            })));

        CreateMap<CardPackage, CardPackageViewModel>()
            .ForMember(dest => dest.Cards, opt => opt.MapFrom(src => src.Cards.Select(card => new CardPackageCreationDto.CardPackageItem
            {
                UserCardId = card.UserCardId,
                Name = card.Name,
                Damage = card.Damage
            })));
    }

}