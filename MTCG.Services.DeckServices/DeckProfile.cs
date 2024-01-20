using AutoMapper;

using MTCG.Domain;
using MTCG.Domain.Cards;
using MTCG.Services.DeckServices.Dto;
using MTCG.Services.DeckServices.ViewModels;

namespace MTCG.Services.DeckServices;

public class DeckProfile : Profile
{

    public DeckProfile()
    {
        CreateMap<Deck, DeckViewModel>();

        CreateMap<DeckCreationDTO, Deck>()
            .ForMember(deck => deck.Cards,
                       expression => expression.MapFrom(deckCreationDto =>
                                                            deckCreationDto.ProvidedUserCardIds.Select(providedUserCardId => new Card { UserCardId = providedUserCardId })));
    }

}