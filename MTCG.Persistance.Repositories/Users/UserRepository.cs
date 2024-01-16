using MTCG.Domain;

namespace MTCG.Persistance.Repositories.users;

public interface UserRepository
{

    ValueTask<User?> GetByUserName(string name);

    ValueTask<User> Create(User user);

    ValueTask<User> Update(User user);

}