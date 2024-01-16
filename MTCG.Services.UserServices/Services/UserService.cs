using MTCG.Domain;

namespace MTCG.Services.UserService;

public interface UserService
{
    ValueTask<User?> GetUserAsync(string username);

    ValueTask<User> RegisterUserAsync(UserRegistrationDto userDto);

    ValueTask<string> LoginUserAsync(UserLoginDto userDto);

}