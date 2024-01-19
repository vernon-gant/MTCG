using System.Data.Common;

using MTCG.Persistence.Database;

using Npgsql;

namespace MTCG.Persistence.Repositories;

public abstract class AbstractRepository
{

    private readonly DatabaseConfig _databaseConfig;

    protected AbstractRepository(DatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    protected DbConnection CreateConnection() => new NpgsqlConnection(_databaseConfig.ConnectionString);

}