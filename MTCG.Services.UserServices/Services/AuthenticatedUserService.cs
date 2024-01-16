using MTCG.Domain;

namespace MTCG.Services.UserService;

public interface AuthenticatedUserService
{
    ValueTask<UserDetailsViewModel> GetUserAsync(string username);

    ValueTask<UserDetailsViewModel> UpdateUserAsync(string userName, UserUpdateDto userUpdateDto);

}