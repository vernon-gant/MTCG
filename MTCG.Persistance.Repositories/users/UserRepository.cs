using MTCG.Domain;

namespace MTCG.Persistance.Repositories.users;

public interface UserRepository
{

    ValueTask<User?> GetByName(string name);

    ValueTask<User> Create(User user);

    ValueTask<User> Update(User user);

}