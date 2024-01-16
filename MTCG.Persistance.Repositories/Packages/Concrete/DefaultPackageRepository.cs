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

    public async ValueTask<CardPackage> Create(CardPackage cardPackage)
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();
            await using DbTransaction transaction = await connection.BeginTransactionAsync();
            await using DbCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO cardpackages (name, createdbyid) VALUES (@name, @createdbyid) RETURNING cardpackageid";
            command.AddParameters(new { name = cardPackage.Name, createdbyid = cardPackage.CreatedById });
            cardPackage.CardPackageId = (int)(await command.ExecuteScalarAsync())!;

            foreach (Card card in cardPackage.Cards)
            {
                command.Parameters.Clear();
                command.CommandText =
                    "INSERT INTO cardpackagecontents (cardpackageid, acquiredcardid, cardid, damage) VALUES (@cardpackageid, @acquiredcardid, @cardid, @damage)";

                command.AddParameters(new
                {
                    cardpackageid = cardPackage.CardPackageId,
                    acquiredcardid = card.UserCardId,
                    cardid = _cardMappings[card.Name].CardId,
                    damage = card.Damage
                });
                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();

            return cardPackage;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating package");

            throw;
        }
    }

    public async ValueTask<CardPackage> Acquire(string username)
    {
        throw new NotImplementedException();
    }

}