using MTCG.Services.UserService.ViewModels;

namespace MTCG.Services.Cards.Services;

public interface CardsService
{

    ValueTask<List<UserCardViewModel>> GetUserCardsAsync(string userName);

}