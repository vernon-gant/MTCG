using System.Data.Common;

using Microsoft.Extensions.Logging;

using MTCG.Domain.Cards;
using MTCG.Domain.Elements;
using MTCG.Persistance.Database;
using MTCG.Persistence.Database;

namespace MTCG.Persistence.Repositories.Cards.Concrete;

public class DefaultCardRepository : AbstractRepository, CardRepository
{

    private readonly ILogger<DefaultCardRepository> _logger;

    public DefaultCardRepository(DatabaseConfig databaseConfig, ILogger<DefaultCardRepository> logger) : base(databaseConfig)
    {
        _logger = logger;
    }


    public async ValueTask<List<Card>> GetUserCardsAsync(int userId)
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();

            List<Element> elements = await GetElementsAsync();

            string cardsSql = "SELECT cards.cardid, usercards.usercardid, cards.name, usercards.damage, cards.elementid " +
                           "FROM usercards " +
                           "INNER JOIN cards ON usercards.cardid = cards.cardid " +
                           "INNER JOIN elements ON cards.elementid = elements.elementid " +
                           "WHERE usercards.userid = @userId";
            List<Card> cards = (await connection.QueryAsync<Card>(cardsSql, new { userid = userId })).ToList();

            foreach (Card card in cards)
            {
                Element foundElement = elements.First(e => e.ElementId == card.ElementId);
                foundElement.Card = card;
                card.Element = Element.FromElement(foundElement);
            }

            return cards;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting user cards");

            throw;
        }
    }

    public async ValueTask<List<Element>> GetElementsAsync()
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();

            string elementsSql = "SELECT * FROM elements";

            return (await connection.QueryAsync<Element>(elementsSql)).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting elements");

            throw;
        }
    }

}