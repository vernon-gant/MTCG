using AutoMapper;

using MTCG.Domain;
using MTCG.Persistence.Repositories.Decks;
using MTCG.Persistence.Repositories.Users;
using MTCG.Services.BattleServices.Battle;
using MTCG.Services.BattleServices.ViewModels;
using MTCG.Services.Cards.Services;
using MTCG.Services.DeckServices.Exceptions;
using MTCG.Services.UserService;

namespace MTCG.Services.BattleServices.Services.Concrete;

public class DefaultBattleService : BattleService
{

    private readonly UserRepository _userRepository;

    private readonly DeckRepository _deckRepository;

    private readonly CardMapperService _cardMapperService;

    private readonly BattleEngine _battleEngine;

    private readonly IMapper _mapper;

    public DefaultBattleService(UserRepository userRepository, DeckRepository deckRepository, CardMapperService cardMapperService, BattleEngine battleEngine, IMapper mapper)
    {
        _userRepository = userRepository;
        _deckRepository = deckRepository;
        _cardMapperService = cardMapperService;
        _battleEngine = battleEngine;
        _mapper = mapper;
    }


    public async ValueTask<BattleResultViewModel> BattleAsync(string userName)
    {
        User? user = await _userRepository.GetByUserName(userName);

        if (user is null) throw new UserNotFoundException();

        Deck? activeDeck = await _deckRepository.GetUserActiveDeckAsync(user.UserId);

        if (activeDeck is null) throw new ActiveDeckNotConfiguredException();

        activeDeck.Cards = await _cardMapperService.MapCardsAsync(activeDeck.Cards);

        BattleResult battleResult = await _battleEngine.BattleAsync(new BattleRequest { User = user, Deck = activeDeck });

        return _mapper.Map<BattleResultViewModel>(battleResult, opt => opt.Items["userName"] = userName);
    }

}