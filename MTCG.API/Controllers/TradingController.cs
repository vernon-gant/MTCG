using MCTG;

using MTCG.API.attributes;
using MTCG.Services.TradingServices.DTO;
using MTCG.Services.TradingServices.Exceptions;
using MTCG.Services.TradingServices.Services;
using MTCG.Services.TradingServices.ViewModels;

namespace MTCG.API.Controllers;

[ApiController(RoutePrefix = "/tradings")]
[Auth]
public class TradingController : ControllerBase
{

    private readonly TradingService _tradingService;

    public TradingController(TradingService tradingService)
    {
        _tradingService = tradingService;
    }

    [Get("")]
    public async ValueTask<ActionResult> GetAvailableTradingDeals(HttpContext httpContext)
    {
        List<TradingDealViewModel> tradingDeals = await _tradingService.GetAvailableTradingDealsAsync();

        if (tradingDeals.Count == 0) return NoContent();

        return Ok(new { tradingDeals });
    }

    [Post("")]
    public async ValueTask<ActionResult> CreateTradingDeal([FromBody] TradingDealCreationDTO tradingDealCreationDTO, HttpContext httpContext)
    {
        try
        {
            await _tradingService.CreateTradingDealAsync(tradingDealCreationDTO, httpContext.UserName!);

            return Created($"/tradings/{tradingDealCreationDTO.TradingDealId}");
        }
        catch (UserCardNotFoundException)
        {
            return NotFound("You do not have the card you want to trade.");
        }
        catch (CardInDeckException)
        {
            return Forbidden("You can't trade a card that is in your deck.");
        }
        catch (CardAlreadyInDealException)
        {
            return Conflict("You can't trade a card that is already in a deal.");
        }
        catch (CardNotOwnedException)
        {
            return Forbidden("You can't trade a card that you don't own.");
        }
        catch (TradingDealIdIsAlreadyTakenException)
        {
            return Conflict("The deal id is already taken.");
        }
    }

    [Post("/{id}")]
    public async ValueTask<ActionResult> CarryOutTradingDeal([FromRoute] Guid id, [FromBody] TradingDealCarryOutDTO tradingDealCarryOutDto, HttpContext httpContext)
    {
        try
        {
            await _tradingService.CarryOutTradingDealAsync(httpContext.UserName!, id, tradingDealCarryOutDto.RespondingUserCardId);

            return Ok();
        }
        catch (UserCardNotFoundException)
        {
            return NotFound("You do not have the card you want to trade.");
        }
        catch (TradingDealNotFoundException)
        {
            return NotFound("The trading deal you want to carry out does not exist.");
        }
        catch (CardAlreadyInDealException)
        {
            return Conflict("You can't trade a card that is already in a deal.");
        }
        catch (SelfDealException)
        {
            return Conflict("You can't trade with yourself.");
        }
        catch (CardInDeckException)
        {
            return Forbidden("You can't trade a card that is in your deck.");
        }
        catch (CardNotOwnedException)
        {
            return Forbidden("You can't trade a card that you don't own.");
        }
        catch (RequiredTypeDoesNotMatchException)
        {
            return Conflict("The required type does not match the card type.");
        }
        catch (RequiredDamageNotReachedException)
        {
            return Conflict("The required damage is not reached.");
        }
    }

    [Delete("/{id}")]
    public async ValueTask<ActionResult> DeleteTradingDeal([FromRoute] Guid id, HttpContext httpContext)
    {
        try
        {
            await _tradingService.DeleteTradingDealAsync(httpContext.UserName!, id);

            return Ok();
        }
        catch (TradingDealNotFoundException)
        {
            return NotFound("The trading deal you want to delete does not exist.");
        }
        catch (TradingDealNotOwnedException)
        {
            return Forbidden("You can't delete a trading deal that you don't own.");
        }
    }

}