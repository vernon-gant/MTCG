using MCTG;

using MTCG.API.attributes;
using MTCG.Services.StatisticsServices.Services;
using MTCG.Services.StatisticsServices.ViewModels;
using MTCG.Services.UserService;

namespace MTCG.API.Controllers;

[ApiController]
[Auth]
public class StatisticsController : ControllerBase
{

    private readonly StatisticsService _statisticsService;

    public StatisticsController(StatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [Get("/stats")]
    public async ValueTask<ActionResult> GetStats(HttpContext context)
    {
        try
        {
            return Ok(await _statisticsService.GetUserStatisticsAsync(context.UserName!));
        }
        catch (UserNotFoundException)
        {
            return NotFound("User with this name does not exist!");
        }
    }

    [Get("/scoreboard")]
    public async ValueTask<ActionResult> GetScoreboard()
    {
        List<UserStatisticsViewModel> scoreboard = await _statisticsService.GetScoreboardAsync();

        if (scoreboard.Count == 0) return NotFound("No users found!");

        return Ok(new { Scoreboard = scoreboard });
    }

}