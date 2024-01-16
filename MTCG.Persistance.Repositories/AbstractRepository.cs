using System.Data.Common;

using Npgsql;

namespace MTCG.Persistance.Database;

public abstract class AbstractRepository
{

    private readonly DatabaseConfig _databaseConfig;

    protected AbstractRepository(DatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    protected DbConnection CreateConnection() => new NpgsqlConnection(_databaseConfig.ConnectionString);

}