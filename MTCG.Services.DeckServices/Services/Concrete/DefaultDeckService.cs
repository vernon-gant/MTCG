using AutoMapper;

using MTCG.Domain;
using MTCG.Persistence.Repositories.Cards;
using MTCG.Persistence.Repositories.Decks;
using MTCG.Persistence.Repositories.Trading;
using MTCG.Persistence.Repositories.Users;
using MTCG.Services.DeckServices.Dto;
using MTCG.Services.DeckServices.Exceptions;
using MTCG.Services.DeckServices.ViewModels;
using MTCG.Services.UserService;
using MTCG.Services.UserService.ViewModels;

namespace MTCG.Services.DeckServices.Services.Concrete;

public class DefaultDeckService : DeckService
{

    private readonly IMapper _mapper;

    private readonly UserRepository _userRepository;

    private readonly CardRepository _cardRepository;

    private readonly DeckRepository _deckRepository;

    private readonly TradingRepository _tradingRepository;

    public DefaultDeckService(UserRepository userRepository, DeckRepository deckRepository, IMapper mapper, CardRepository cardRepository, TradingRepository tradingRepository)
    {
        _userRepository = userRepository;
        _deckRepository = deckRepository;
        _cardRepository = cardRepository;
        _tradingRepository = tradingRepository;
        _mapper = mapper;
    }

    public async ValueTask<DeckViewModel?> GetUserDeckByIdAsync(string userName, int deckId)
    {
        User? user = await _userRepository.GetByUserName(userName);

        if (user is null) throw new UserNotFoundException();

        Deck? deck = await _deckRepository.GetByIdAsync(deckId);

        if (deck is null) return null;

        if (deck.UserId != user.UserId) throw new DeckNotOwnedException();

        return _mapper.Map<DeckViewModel>(deck);
    }

    public async ValueTask<List<DeckViewModel>> GetUserDecksAsync(string userName)
    {
        User? user = await _userRepository.GetByUserName(userName);

        if (user is null) throw new UserNotFoundException();

        List<Deck> decks = await _deckRepository.GetUserDecksAsync(user.UserId);

        return _mapper.Map<List<DeckViewModel>>(decks);
    }

    public async ValueTask<DeckViewModel?> GetUserActiveDeckAsync(string userName)
    {
        User? user = await _userRepository.GetByUserName(userName);

        if (user is null) throw new UserNotFoundException();

        Deck? activeDeck = await _deckRepository.GetUserActiveDeckAsync(user.UserId);

        return _mapper.Map<DeckViewModel?>(activeDeck);
    }

    public async ValueTask<DeckViewModel> AddUserDeckAsync(string userName, DeckCreationDTO deckCreationDto)
    {
        User? user = await _userRepository.GetByUserName(userName);

        if (user is null) throw new UserNotFoundException();

        await ValidateAddDeckInvariants(deckCreationDto, user);

        Deck deck = _mapper.Map<Deck>(deckCreationDto);
        deck.UserId = user.UserId;

        Deck addedDeck = await _deckRepository.AddDeckAsync(deck);

        return _mapper.Map<DeckViewModel>(addedDeck);
    }

    private async Task ValidateAddDeckInvariants(DeckCreationDTO deckCreationDto, User user)
    {
        List<Card> userCards = await _cardRepository.GetUserCardsAsync(user.UserId);

        List<Guid> notInUserStackCardIds =
            deckCreationDto.ProvidedUserCardIds.Where(providedUserCardId => userCards.All(userCard => userCard.UserCardId != providedUserCardId)).ToList();

        if (notInUserStackCardIds.Count > 0) throw new CardNotInUserStackException(notInUserStackCardIds);

        List<Deck> userDecks = await _deckRepository.GetUserDecksAsync(user.UserId);

        List<Guid> cardsAlreadyInDeck = deckCreationDto.ProvidedUserCardIds
                                                       .Where(providedUserCardId =>
                                                                  userDecks.Any(userDeck => userDeck.Cards.Any(userDeckCard => userDeckCard.UserCardId == providedUserCardId)))
                                                       .ToList();

        if (cardsAlreadyInDeck.Count > 0) throw new CardAlreadyInDeckException(cardsAlreadyInDeck);

        List<TradingDeal> tradingDeals = await _tradingRepository.GetAvailableAsync();

        List<Guid> cardsAlreadyInDeal = deckCreationDto.ProvidedUserCardIds
                                                      .Where(providedUserCardId =>
                                                                 tradingDeals.Any(tradingDeal => tradingDeal.OfferingUserCardId == providedUserCardId))
                                                      .ToList();

        if (cardsAlreadyInDeal.Count > 0) throw new CardAlreadyInTradingDealException(cardsAlreadyInDeal);
    }

    public async ValueTask<DeckViewModel> SetUserActiveDeckAsync(string userName, int deckId)
    {
        User? user = await _userRepository.GetByUserName(userName);

        if (user is null) throw new UserNotFoundException();

        Deck? activeDeck = await _deckRepository.GetUserActiveDeckAsync(user.UserId);

        if (activeDeck is not null) throw new ActiveDeckAlreadyConfiguredException();

        Deck? deck = await _deckRepository.GetByIdAsync(deckId);

        if (deck is null) throw new DeckNotFoundException();

        if (deck.UserId != user.UserId) throw new DeckNotOwnedException();

        await _deckRepository.SetActiveDeckAsync(deckId);

        deck.IsActive = true;

        return _mapper.Map<DeckViewModel>(deck);
    }

    public async Task UnsetUserActiveDeckAsync(string userName)
    {
        User? user = await _userRepository.GetByUserName(userName);

        if (user is null) throw new UserNotFoundException();

        Deck? activeDeck = await _deckRepository.GetUserActiveDeckAsync(user.UserId);

        if (activeDeck is null) throw new ActiveDeckNotConfiguredException();

        await _deckRepository.UnsetActiveDeckAsync(user.UserId);
    }

}