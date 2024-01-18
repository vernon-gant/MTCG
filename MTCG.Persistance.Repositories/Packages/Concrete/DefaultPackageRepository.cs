using System.Data.Common;

using Microsoft.Extensions.Logging;

using MTCG.Domain;
using MTCG.Persistance.Database;

namespace MTCG.Persistance.Repositories.Packages.Concrete;

public class DefaultPackageRepository : AbstractRepository, PackageRepository
{

    private readonly Dictionary<string, CardMapping> _cardMappings;

    private readonly ILogger<DefaultPackageRepository> _logger;

    public DefaultPackageRepository(DatabaseConfig databaseConfig, ILogger<DefaultPackageRepository> logger, Dictionary<string, CardMapping> cardMappings) : base(databaseConfig)
    {
        _cardMappings = cardMappings;
        _logger = logger;
    }

    public async ValueTask<Package> Create(Package package)
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();
            await using DbTransaction transaction = await connection.BeginTransactionAsync();
            await using DbCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO packages (name, createdbyid) VALUES (@name, @createdbyid) RETURNING packageid";
            command.AddParameters(new { name = package.Name, createdbyid = package.CreatedById });
            package.PackageId = (int)(await command.ExecuteScalarAsync())!;

            foreach (Card card in package.Cards)
            {
                command.Parameters.Clear();
                command.CommandText =
                    "INSERT INTO packagecontents (packageid, packagecardid, cardid, damage) VALUES (@packageid, @packagecardid, @cardid, @damage)";

                command.AddParameters(new
                {
                    packageid = package.PackageId,
                    packagecardid = card.UserCardId,
                    cardid = _cardMappings[card.Name].CardId,
                    damage = card.Damage
                });
                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();

            return package;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating package");
            throw;
        }
    }

    public async ValueTask<Package?> GetFirstNotAcquiredPackage()
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();

            Package? cardPackage = await connection.QuerySingleAsync<Package>(
                "SELECT * FROM packages WHERE packages.acquiredbyid IS NULL ORDER BY createdon ASC LIMIT 1");

            if (cardPackage is null) return null;

            IEnumerable<Card> cards = await connection.QueryAsync<Card>(
                "SELECT packagecardid as usercardid, cards.cardid, name, damage FROM packagecontents INNER JOIN cards ON packagecontents.cardid = cards.cardid WHERE packagecontents.packageid = @packageid",
                new { packageid = cardPackage.PackageId });

            cardPackage.Cards = cards.ToList();

            return cardPackage;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting first not acquired package");

            throw;
        }
    }

    public async ValueTask<Package> AddPackageToUser(int userId, Package package, int packagePrice)
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();
            await using DbTransaction transaction = await connection.BeginTransactionAsync();
            await using DbCommand command = connection.CreateCommand();

            foreach (Card card in package.Cards)
            {
                command.Parameters.Clear();
                command.CommandText = "INSERT INTO usercards (usercardid, userid, cardid, damage) VALUES (@usercardid, @userid, @cardid, @damage)";
                command.AddParameters(new { usercardid = card.UserCardId, userid = userId, cardid = _cardMappings[card.Name].CardId, damage = card.Damage });
                await command.ExecuteNonQueryAsync();
            }

            command.Parameters.Clear();
            command.CommandText = "UPDATE packages SET acquiredbyid = @acquiredbyid, acquiredon = NOW() WHERE packageid = @packageid";
            command.AddParameters(new { acquiredbyid = userId, packageid = package.PackageId });
            await command.ExecuteNonQueryAsync();

            command.Parameters.Clear();
            command.CommandText = "UPDATE users SET coins = coins - @packageprice WHERE userid = @userid";
            command.AddParameters(new { userid = userId, packageprice = packagePrice });
            await command.ExecuteNonQueryAsync();

            await transaction.CommitAsync();

            return package;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while adding package to user");

            throw;
        }
    }

}