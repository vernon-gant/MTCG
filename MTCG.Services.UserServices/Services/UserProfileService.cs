namespace MTCG.Services.UserService.Services;

public interface UserProfileService
{
    ValueTask<UserDetailsViewModel> GetUserAsync(string username);

    ValueTask<UserDetailsViewModel> UpdateUserAsync(string userName, UserUpdateDto userUpdateDto);

}