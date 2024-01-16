using System.Data.Common;

using MTCG.Domain;
using MTCG.Persistance.Database;

namespace MTCG.Persistance.Repositories;

public class DefaultCardRepository : AbstractRepository, CardsRepository
{

    private readonly List<CardMapping> _cardMappings;

    private readonly List<ElementMapping> _elementMappings;

    public DefaultCardRepository(DatabaseConfig databaseConfig) : base(databaseConfig)
    {
        _cardMappings = new List<CardMapping>();
        _elementMappings = new List<ElementMapping>();
    }

    public async ValueTask<IEnumerable<CardDto>> GetAllCards()
    {
        await using DbConnection connection = CreateConnection();
        await connection.OpenAsync();
        await using DbCommand command = connection.CreateCommand();

        command.CommandText = "SELECT cards.name, elements.name as elementname FROM cards JOIN elements ON cards.elementid = elements.elementid";

        return await connection.QueryAsync<CardDto>(command.CommandText);
    }

    public async ValueTask<IEnumerable<Card>> GetUserCards(string username)
    {
        throw new NotImplementedException();
    }

}