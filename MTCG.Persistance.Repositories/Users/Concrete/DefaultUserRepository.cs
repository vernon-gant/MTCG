using System.Data.Common;

using Microsoft.Extensions.Logging;

using MTCG.Domain;
using MTCG.Persistance.Database;

namespace MTCG.Persistance.Repositories.users;

public class DefaultUserRepository : AbstractRepository, UserRepository
{

    private readonly ILogger<DefaultUserRepository> _logger;

    public DefaultUserRepository(DatabaseConfig databaseConfig, ILogger<DefaultUserRepository> logger) : base(databaseConfig)
    {
        _logger = logger;
    }

    public async ValueTask<User?> GetByUserName(string name)
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();
            User? user = await connection.QuerySingleAsync<User>("SELECT * FROM users WHERE username = @name AND deletedon IS NULL", new { name });

            return user;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting user by name");
            throw;
        }
    }

    public async ValueTask<User> Create(User user)
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();
            await using DbCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO users (username, password) VALUES (@username, @password)";
            command.AddParameters(new { username = user.UserName, password = user.Password });
            await command.ExecuteNonQueryAsync();

            return connection.QuerySingleAsync<User>("SELECT * FROM users WHERE username = @username", new { username = user.UserName }).Result!;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating user");
            throw;
        }
    }

    public async ValueTask<User> Update(User user)
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();
            await using DbCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE users SET name = @name, bio = @bio, image = @image WHERE username = @username";
            command.AddParameters(new { name = user.Name, bio = user.BIO, image = user.Image, username = user.UserName });
            await command.ExecuteNonQueryAsync();

            return connection.QuerySingleAsync<User>("SELECT * FROM users WHERE username = @username", new { username = user.UserName }).Result!;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating user");
            throw;
        }
    }

}