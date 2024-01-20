using System.Data.Common;

using Microsoft.Extensions.Logging;

using MTCG.Domain;
using MTCG.Domain.Cards;
using MTCG.Domain.Elements;
using MTCG.Persistance.Database;
using MTCG.Persistence.Database;
using MTCG.Persistence.Repositories.Cards;

namespace MTCG.Persistence.Repositories.Decks.Concrete;

public class DefaultDeckRepository : AbstractRepository, DeckRepository
{

    private readonly CardRepository _cardRepository;

    private readonly ILogger<DefaultDeckRepository> _logger;

    public DefaultDeckRepository(DatabaseConfig databaseConfig, ILogger<DefaultDeckRepository> logger, CardRepository cardRepository) : base(databaseConfig)
    {
        _logger = logger;
        _cardRepository = cardRepository;
    }

    public async ValueTask<List<Deck>> GetUserDecksAsync(int userId)
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();

            string decksSql = "SELECT * FROM decks WHERE userid = @userid";
            List<Deck> decks = (await connection.QueryAsync<Deck>(decksSql, new { userid = userId })).ToList();

            foreach (Deck deck in decks)
            {
                deck.Cards = await QueryDeckCards(deck.DeckId, connection);
            }

            return decks;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting user decks");

            throw;
        }
    }

    public async ValueTask<Deck?> GetByIdAsync(int deckId)
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();

            string deckSql = "SELECT * FROM decks WHERE deckid = @deckid";
            Deck? deck = await connection.QuerySingleAsync<Deck>(deckSql, new { deckid = deckId });

            if (deck is null) return null;

            deck.Cards = await QueryDeckCards(deck.DeckId, connection);

            return deck;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting user deck");

            throw;
        }
    }

    public async ValueTask<Deck?> GetUserActiveDeckAsync(int userId)
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();

            string activeDeckSql = "SELECT * FROM decks WHERE userid = @userid AND isactive = true";
            Deck? activeDeck = await connection.QuerySingleAsync<Deck>(activeDeckSql, new { userid = userId });

            if (activeDeck is null) return null;

            activeDeck.Cards = await QueryDeckCards(activeDeck.DeckId, connection);

            return activeDeck;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting user deck");

            throw;
        }
    }

    public async ValueTask<Deck> AddDeckAsync(Deck deckToAdd)
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();
            await using DbTransaction transaction = await connection.BeginTransactionAsync();

            string addDeckSql = "INSERT INTO decks (userid, description) VALUES (@userid, @description) RETURNING deckid";
            Deck deckEntry = await connection.QuerySingleAsync<Deck>(addDeckSql, new { userid = deckToAdd.UserId, description = deckToAdd.Description })!;
            deckToAdd.DeckId = deckEntry!.DeckId;

            await AddDeckContents(deckToAdd, connection);

            await transaction.CommitAsync();

            Deck? addedDeck = await GetByIdAsync(deckEntry.DeckId);

            return addedDeck!;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while adding user deck");

            throw;
        }
    }

    public async Task SetActiveDeckAsync(int deckId)
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();
            await using DbCommand command = connection.CreateCommand();

            string activateDeckSql = "UPDATE decks SET isactive = true WHERE deckid = @deckid";
            command.CommandText = activateDeckSql;
            command.AddParameters(new { deckid = deckId });
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while setting user active deck");

            throw;
        }
    }

    public async Task UnsetActiveDeckAsync(int userId)
    {
        try
        {
            await using DbConnection connection = CreateConnection();
            await connection.OpenAsync();
            await using DbCommand command = connection.CreateCommand();

            string deactivateDeckSql = "UPDATE decks SET isactive = false WHERE userid = @userid AND isactive = true";
            command.CommandText = deactivateDeckSql;
            command.AddParameters(new { userid = userId });
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while unsetting user active deck");

            throw;
        }
    }

    private async Task AddDeckContents(Deck deckToAdd, DbConnection connection)
    {
        await using DbCommand command = connection.CreateCommand();
        string addDeckCardsSql = "INSERT INTO deckcontents (deckid, usercardid) VALUES (@deckid, @usercardid)";

        foreach (Card cardToAdd in deckToAdd.Cards)
        {
            command.CommandText = addDeckCardsSql;
            command.AddParameters(new { deckid = deckToAdd.DeckId, usercardid = cardToAdd.UserCardId });
            await command.ExecuteNonQueryAsync();
            command.Parameters.Clear();
        }
    }

    private async ValueTask<List<Card>> QueryDeckCards(int deckId, DbConnection connection)
    {
        string activeDeckCardsSql = "SELECT cards.cardid, usercards.usercardid, cards.name, usercards.damage, cards.elementid " +
                                    "FROM decks " +
                                    "INNER JOIN deckcontents ON decks.deckid = deckcontents.deckid " +
                                    "INNER JOIN usercards ON deckcontents.usercardid = usercards.usercardid " +
                                    "INNER JOIN cards ON usercards.cardid = cards.cardid " +
                                    "INNER JOIN elements ON cards.elementid = elements.elementid " +
                                    "WHERE decks.deckid = @deckid";

        List<Card> cards = (await connection.QueryAsync<Card>(activeDeckCardsSql, new { deckid = deckId })).ToList();

        List<Element> elements = await _cardRepository.GetElementsAsync();

        foreach (Card card in cards)
        {
            card.Element = elements.First(e => e.ElementId == card.ElementId);
        }

        return cards;
    }

}