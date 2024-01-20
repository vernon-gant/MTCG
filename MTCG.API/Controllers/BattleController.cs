using MCTG;

using MTCG.API.attributes;
using MTCG.Services.BattleServices.Services;

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
        return Ok();
    }

}