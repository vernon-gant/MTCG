using MCTG;

using MTCG.API.attributes;
using MTCG.Domain;
using MTCG.Services.UserService;

namespace MTCG.API.Controllers;

[ApiController]
public class UserController : ControllerBase
{

    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [Post("/users")]
    public async ValueTask<ActionResult> Register([FromBody] UserRegistrationDto userDto)
    {
        if (_userService.Get(userDto.UserName).Result != null) return Conflict("User already exists!");

        await _userService.Register(userDto);

        return Created("users/" + userDto.UserName);
    }

    [Post("/sessions")]
    public async ValueTask<ActionResult> Login([FromBody] UserLoginDto userDto)
    {
        try
        {
            string token = await _userService.Login(userDto);

            return Ok(new { token });
        }
        catch (UserNotFoundException)
        {
            return Unauthorized("User with this name does not exist!");
        }
        catch (WrongPasswordException)
        {
            return Unauthorized("Wrong password!");
        }
    }

}