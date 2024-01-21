using MCTG;

using MTCG.API.attributes;
using MTCG.Services.BattleServices.Services;
using MTCG.Services.BattleServices.ViewModels;
using MTCG.Services.DeckServices.Exceptions;
using MTCG.Services.UserService;

namespace MTCG.API.Controllers;

[ApiController]
[Auth]
public class BattleController : ControllerBase
{

    private readonly BattleService _battleService;

    public BattleController(BattleService battleService)
    {
        _battleService = battleService;
    }

    [Post("/battles")]
    public async ValueTask<ActionResult> Battle(HttpContext context)
    {
        try
        {
            BattleResultViewModel battleResultViewModel = await _battleService.BattleAsync(context.UserName!);

            return Ok(new { battleResult = battleResultViewModel });
        }
        catch (UserNotFoundException)
        {
            return BadRequest("User not found");
        }
        catch (ActiveDeckNotConfiguredException)
        {
            return BadRequest("Active deck not configured");
        }
    }

}