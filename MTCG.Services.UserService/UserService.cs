using MTCG.Domain;

namespace MTCG.Services.UserService;

public interface UserService
{
    ValueTask<User?> Get(string username);

    ValueTask<User> Register(UserRegistrationDto userDto);

    ValueTask<string> Login(UserLoginDto userDto);

}