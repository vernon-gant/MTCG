using MTCG.Domain;

namespace MTCG.Persistence.Repositories.Users;

public interface UserRepository
{

    ValueTask<User?> GetByUserName(string name);

    ValueTask<User> Create(User user);

    ValueTask<User> Update(User user);

}