using AutoMapper;

using MTCG.Domain;
using MTCG.Persistence.Repositories.Cards;
using MTCG.Persistence.Repositories.Users;
using MTCG.Services.UserService;
using MTCG.Services.UserService.ViewModels;

namespace MTCG.Services.Cards.Services.Concrete;

public class DefaultCardsService : CardsService
{

    private readonly UserRepository _userRepository;

    private readonly CardRepository _cardRepository;

    private readonly IMapper _mapper;

    public DefaultCardsService(CardRepository cardRepository, UserRepository userRepository, IMapper mapper)
    {
        _cardRepository = cardRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async ValueTask<List<UserCardViewModel>> GetUserCardsAsync(string userName)
    {
        User? foundUser = await _userRepository.GetByUserName(userName);

        if (foundUser is null) throw new UserNotFoundException();

        List<Card> userCards = await _cardRepository.GetUserCardsAsync(foundUser.UserId);

        return _mapper.Map<List<UserCardViewModel>>(userCards);
    }

}