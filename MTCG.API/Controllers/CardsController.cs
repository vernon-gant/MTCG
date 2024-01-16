using MCTG;

using MTCG.API.attributes;
using MTCG.Services.Cards.cards;

namespace MTCG.API.Controllers;

[ApiController]
public class CardsController : ControllerBase
{

    private readonly CardsService _cardsService;

    public CardsController(CardsService cardsService)
    {
        _cardsService = cardsService;
    }

    [Get("/all-cards")]
    public async ValueTask<ActionResult> GetAllCards()
    {
        return Ok(await _cardsService.GetAllCards());
    }

    [Get("/cards/{id}")]
    public ValueTask<ActionResult> GetCardById([FromRoute] int id)
    {
        throw new NotImplementedException();
    }

}