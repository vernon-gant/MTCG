using MCTG;

using MTCG.API.attributes;
using MTCG.Services.DeckServices.Dto;
using MTCG.Services.DeckServices.Exceptions;
using MTCG.Services.DeckServices.Services;
using MTCG.Services.DeckServices.ViewModels;
using MTCG.Services.UserService;
using MTCG.Services.UserService.ViewModels;

namespace MTCG.API.Controllers;

[ApiController]
[Auth]
public class DeckController : ControllerBase
{

    private readonly DeckService _deckService;

    public DeckController(DeckService deckService)
    {
        _deckService = deckService;
    }

    [Get("/decks")]
    public async ValueTask<ActionResult> GetUserDecks(HttpContext context)
    {
        try
        {
            List<DeckViewModel> userDecks = await _deckService.GetUserDecksAsync(context.UserName!);

            if (userDecks.Count == 0) return NoContent();

            return Ok(new { decks = userDecks });
        }
        catch (UserNotFoundException)
        {
            return NotFound("User with this name does not exist!");
        }
    }

    [Get("/decks/{deckid}")]
    public async ValueTask<ActionResult > GetUserDeckById([FromRoute] int deckId, HttpContext context)
    {
        try
        {
            DeckViewModel? deck = await _deckService.GetUserDeckByIdAsync(context.UserName!, deckId);

            if (deck is null) return NotFound("Deck with this id does not exist!");

            return Ok(deck);
        }
        catch (UserNotFoundException)
        {
            return NotFound("User with this name does not exist!");
        }
        catch (DeckNotOwnedException)
        {
            return Forbidden("You are not allowed to access this deck!");
        }
    }

    [Get("/active-deck")]
    public async ValueTask<ActionResult> GetUserActiveDeck(HttpContext context)
    {
        try
        {
            DeckViewModel? activeDeck = await _deckService.GetUserActiveDeckAsync(context.UserName!);

            if (activeDeck is null) return NoContent();

            return Ok(activeDeck);
        }
        catch (UserNotFoundException)
        {
            return NotFound("User with this name does not exist!");
        }
    }

    [Put("/decks")]
    public async ValueTask<ActionResult> AddDeckToUser(HttpContext context, [FromBody] DeckCreationDTO deckCreationDto)
    {
        try
        {
            DeckViewModel addedDeck = await _deckService.AddUserDeckAsync(context.UserName!, deckCreationDto);

            return Ok(addedDeck);
        }
        catch (UserNotFoundException)
        {
            return NotFound("User with this name does not exist!");
        }
        catch (CardNotInUserStackException e)
        {
            return BadRequest(new { errorMessage = "Card with these ids are not in your stack!", cardIds = e.CardIds });
        }
        catch (CardAlreadyInDeckException e)
        {
            return BadRequest(new { errorMessage = "Card with these ids are already in your deck!", cardIds = e.CardIds });
        }
    }

    [Post("/active-deck/{deckid}")]
    public async ValueTask<ActionResult> SetUserActiveDeck([FromRoute] int deckId, HttpContext context)
    {
        try
        {
            DeckViewModel deck = await _deckService.SetUserActiveDeckAsync(context.UserName!, deckId);

            return Ok(deck);
        }
        catch (UserNotFoundException)
        {
            return NotFound("User with this name does not exist!");
        }
        catch(DeckNotFoundException)
        {
            return NotFound("Deck with this id does not exist!");
        }
        catch (DeckNotOwnedException)
        {
            return Forbidden("You are not allowed to access this deck!");
        }
        catch (ActiveDeckAlreadyConfiguredException)
        {
            return BadRequest("Active deck is already configured!");
        }
    }

    [Delete("/active-deck")]
    public async ValueTask<ActionResult> UnsetUserActiveDeck(HttpContext context)
    {
        try
        {
            await _deckService.UnsetUserActiveDeckAsync(context.UserName!);

            return Ok();
        }
        catch (UserNotFoundException)
        {
            return NotFound("User with this name does not exist!");
        }
        catch (ActiveDeckNotConfiguredException)
        {
            return BadRequest("Active deck is not configured!");
        }
    }

}