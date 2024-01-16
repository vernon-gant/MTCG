using System.Data.Common;

using Microsoft.Extensions.Logging;

using MTCG.Domain;
using MTCG.Persistance.Database;

namespace MTCG.Persistance.Repositories;

public class DefaultCardRepository : AbstractRepository, CardsRepository
{

    private readonly ILogger<DefaultCardRepository> _logger;

    private readonly Dictionary<string, CardMapping> _cardMappings;

    private readonly Dictionary<string, ElementMapping> _elementMappings;

    public DefaultCardRepository(DatabaseConfig databaseConfig, ILogger<DefaultCardRepository> logger, Dictionary<string, CardMapping> cardMappings,
        Dictionary<string, ElementMapping> elementMappings) : base(databaseConfig)
    {
        _logger = logger;
        _cardMappings = cardMappings;
        _elementMappings = elementMappings;
    }


    public async ValueTask<Card?> GetCardByUserCardIdAsync(Guid userCardId)
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            Card? card = await connection.QuerySingleAsync<Card>("SELECT * FROM usercards WHERE usercardid = @userCardId", new { userCardId });
            return card;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting card by userCardId");
            throw;
        }
    }

}