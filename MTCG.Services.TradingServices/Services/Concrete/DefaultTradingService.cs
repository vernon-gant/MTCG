using AutoMapper;

using MTCG.Domain;
using MTCG.Persistence.Repositories.Cards;
using MTCG.Persistence.Repositories.Decks;
using MTCG.Persistence.Repositories.Trading;
using MTCG.Persistence.Repositories.Users;
using MTCG.Services.TradingServices.DTO;
using MTCG.Services.TradingServices.Exceptions;
using MTCG.Services.TradingServices.ViewModels;
using MTCG.Services.UserService;

namespace MTCG.Services.TradingServices.Services.Concrete;

public class DefaultTradingService : TradingService
{

    private readonly UserRepository _userRepository;

    private readonly TradingRepository _tradingRepository;

    private readonly CardRepository _cardRepository;

    private readonly DeckRepository _deckRepository;

    private readonly IMapper _mapper;

    public DefaultTradingService(TradingRepository tradingRepository, IMapper mapper, UserRepository userRepository, CardRepository cardRepository, DeckRepository deckRepository)
    {
        _tradingRepository = tradingRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _cardRepository = cardRepository;
        _deckRepository = deckRepository;
    }

    public async ValueTask<List<TradingDealViewModel>> GetAvailableTradingDealsAsync()
    {
        List<TradingDeal> tradingDeals = await _tradingRepository.GetAvailableAsync();

        return _mapper.Map<List<TradingDealViewModel>>(tradingDeals);
    }

    public async Task CreateTradingDealAsync(TradingDealCreationDTO tradingDealCreationDTO, string userName)
    {
        User? foundUser = await _userRepository.GetByUserName(userName);

        if (foundUser == null) throw new UserNotFoundException();

        if (!await IsCardOwnedByUser(tradingDealCreationDTO.OfferingUserCardId, foundUser.UserId)) throw new CardNotOwnedException();

        List<TradingDeal> tradingDeals = await _tradingRepository.GetAvailableAsync();

        if (TradingDealIdIsAlreadyTaken(tradingDealCreationDTO.TradingDealId, tradingDeals)) throw new TradingDealIdIsAlreadyTakenException();

        if (IsCardInDeal(tradingDealCreationDTO.OfferingUserCardId, tradingDeals)) throw new CardAlreadyInDealException();

        if (await IsCardInDeck(tradingDealCreationDTO.OfferingUserCardId, foundUser.UserId)) throw new CardInDeckException();

        TradingDeal tradingDeal = _mapper.Map<TradingDeal>(tradingDealCreationDTO);

        tradingDeal.OfferingUserId = foundUser.UserId;

        await _tradingRepository.CreateAsync(tradingDeal);
    }

    private async ValueTask<bool> IsCardInDeck(Guid cardId, int userId)
    {
        List<Deck> userDecks = await _deckRepository.GetUserDecksAsync(userId);

        foreach (Deck deck in userDecks)
        {
            if (deck.Cards.Any(card => card.UserCardId == cardId)) return true;
        }

        return false;
    }

    private async ValueTask<bool> IsCardOwnedByUser(Guid cardId, int userId)
    {
        List<Card> userCards = await _cardRepository.GetUserCardsAsync(userId);

        return userCards.Any(card => card.UserCardId == cardId);
    }

    private bool TradingDealIdIsAlreadyTaken(Guid tradingDealId, List<TradingDeal> tradingDeals) => tradingDeals.Any(tradingDeal => tradingDeal.TradingDealId == tradingDealId);

    private bool IsCardInDeal(Guid cardId, List<TradingDeal> tradingDeals) => tradingDeals.Any(tradingDeal => tradingDeal.OfferingUserCardId == cardId);

}