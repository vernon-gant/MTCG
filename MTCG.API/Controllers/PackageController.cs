using MCTG;

using MTCG.API.attributes;
using MTCG.Services.PackageServices.Exceptions;
using MTCG.Services.PackageServices.Services;
using MTCG.Services.PackageServices.ViewModels;
using MTCG.Services.UserService;

namespace MTCG.API.Controllers;

[ApiController]
[Auth]
public class PackageController : ControllerBase
{

    private readonly PackageService _packageService;

    public PackageController(PackageService packageService)
    {
        _packageService = packageService;
    }

    [Post("/transactions/packages")]
    public async ValueTask<ActionResult> AcquirePackage(HttpContext context)
    {
        try
        {
            CardPackageViewModel acquiredPackage = await _packageService.AcquirePackageAsync(context.UserName!);

            return Ok(acquiredPackage);
        }
        catch (UserNotFoundException)
        {
            return NotFound("User with this name does not exist!");
        }
        catch (NotEnoughCoinsException)
        {
            return Forbidden("You do not have enough coins to buy a package!");
        }
        catch (NoPackageAvailableException)
        {
            return NotFound("There are no packages available for you to buy!");
        }
    }

}