using MCTG;

using MTCG.API.attributes;
using MTCG.Services.StatisticsServices.Services;
using MTCG.Services.UserService;
using MTCG.Services.UserService.Services;

namespace MTCG.API.Controllers;

[Auth]
[ApiController]
public class UserProfileController : ControllerBase
{

    private readonly UserProfileService _userProfileService;

    private readonly StatisticsService _statisticsService;

    public UserProfileController(UserProfileService userProfileService, StatisticsService statisticsService)
    {
        _userProfileService = userProfileService;
        _statisticsService = statisticsService;
    }

    [Get("/users/{username}")]
    public async ValueTask<ActionResult> GetInfo([FromRoute] string userName, HttpContext context)
    {
        try {
            if (IsUnauthorizedAccess(context,userName)) return Unauthorized("You are not allowed to access this resource!");
            return Ok(await _userProfileService.GetUserAsync(userName));
        }
        catch (UserNotFoundException)
        {
            return NotFound("User with this name does not exist!");
        }
    }

    [Put("/users/{username}")]
    public async ValueTask<ActionResult> UpdateInfo([FromRoute] string userName, [FromBody] UserUpdateDto userUpdateDto, HttpContext context)
    {
        try {
            if (IsUnauthorizedAccess(context,userName)) return Unauthorized("You are not allowed to access this resource!");
            return Ok(await _userProfileService.UpdateUserAsync(userName, userUpdateDto));
        }
        catch (UserNotFoundException)
        {
            return NotFound("User with this name does not exist!");
        }
    }


    private bool IsUnauthorizedAccess(HttpContext context, string userName) => userName != context.UserName && !context.IsAdmin;

}