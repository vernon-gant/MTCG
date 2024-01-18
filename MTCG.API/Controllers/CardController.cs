using MCTG;

using MTCG.API.attributes;
using MTCG.Services.Cards.cards;

namespace MTCG.API.Controllers;

[ApiController]
public class CardController : ControllerBase
{

    private readonly CardsService _cardsService;

    public CardController(CardsService cardsService)
    {
        _cardsService = cardsService;
    }

    [Get("/all-cards")]
    public async ValueTask<ActionResult> GetAllCards()
    {
        throw new NotImplementedException();
    }

    [Get("/cards/{id}")]
    public ValueTask<ActionResult> GetCardById([FromRoute] int id)
    {
        throw new NotImplementedException();
    }

}