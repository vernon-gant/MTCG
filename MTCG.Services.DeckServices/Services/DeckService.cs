using MTCG.Services.DeckServices.Dto;
using MTCG.Services.DeckServices.ViewModels;
using MTCG.Services.UserService.ViewModels;

namespace MTCG.Services.DeckServices.Services;

public interface DeckService
{

    ValueTask<DeckViewModel?> GetUserDeckByIdAsync(string userName, int deckId);

    ValueTask<DeckViewModel?> GetUserActiveDeckAsync(string userName);

    ValueTask<List<DeckViewModel>> GetUserDecksAsync(string userName);

    ValueTask<DeckViewModel> AddUserDeckAsync(string userName, DeckCreationDTO deckCreationDto);

    ValueTask<DeckViewModel> SetUserActiveDeckAsync(string userName, int deckId);

    Task UnsetUserActiveDeckAsync(string userName);

}