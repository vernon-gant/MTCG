using MCTG;

using MTCG.API.attributes;
using MTCG.Services.Cards.Services;
using MTCG.Services.UserService;
using MTCG.Services.UserService.ViewModels;

namespace MTCG.API.Controllers;

[ApiController]
[Auth]
public class CardController : ControllerBase
{

    private readonly CardsService _cardsService;

    public CardController(CardsService cardsService)
    {
        _cardsService = cardsService;
    }

    [Get("/cards")]
    public async ValueTask<ActionResult> GetUserCards(HttpContext context)
    {
        try
        {
            List<UserCardViewModel> userCards = await _cardsService.GetUserCardsAsync(context.UserName!);

            if (userCards.Count == 0) return NoContent();

            return Ok(new { cards = userCards });
        }
        catch (UserNotFoundException)
        {
            return NotFound("User with this name does not exist!");
        }
    }

}