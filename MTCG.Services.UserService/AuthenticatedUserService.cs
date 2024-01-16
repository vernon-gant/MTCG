using MTCG.Domain;

namespace MTCG.Services.UserService;

public interface AuthenticatedUserService
{
    ValueTask<UserDetails> Get(string username);

    ValueTask<User> Update(string userName, UserUpdateDto userUpdateDto);

}