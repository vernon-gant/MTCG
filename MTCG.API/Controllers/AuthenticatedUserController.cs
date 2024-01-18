using MCTG;

using MTCG.API.attributes;
using MTCG.Services.UserService;

namespace MTCG.API.Controllers;

[Auth]
[ApiController]
public class AuthenticatedUserController : ControllerBase
{

    private readonly AuthenticatedUserService _authenticatedUserService;

    public AuthenticatedUserController(AuthenticatedUserService userService)
    {
        _authenticatedUserService = userService;
    }

    [Get("/users/{username}")]
    public async ValueTask<ActionResult> GetUserInfo([FromRoute] string userName, HttpContext context)
    {
        try {
            if (IsUnauthorizedAccess(context,userName)) return Unauthorized("You are not allowed to access this resource!");
            return Ok(await _authenticatedUserService.GetUserAsync(userName));
        }
        catch (UserNotFoundException)
        {
            return NotFound("User with this name does not exist!");
        }
    }

    [Put("/users/{username}")]
    public async ValueTask<ActionResult> UpdateUserInfo([FromRoute] string userName, [FromBody] UserUpdateDto userUpdateDto, HttpContext context)
    {
        try {
            if (IsUnauthorizedAccess(context,userName)) return Unauthorized("You are not allowed to access this resource!");
            return Ok(await _authenticatedUserService.UpdateUserAsync(userName, userUpdateDto));
        }
        catch (UserNotFoundException)
        {
            return NotFound("User with this name does not exist!");
        }
    }

    private bool IsUnauthorizedAccess(HttpContext context, string userName) => userName != context.UserName && !context.IsAdmin;

}