using MCTG;

using MTCG.API.attributes;
using MTCG.Domain;
using MTCG.Services.PackageServices.Dto;
using MTCG.Services.PackageServices.Exceptions;
using MTCG.Services.PackageServices.Services;
using MTCG.Services.PackageServices.ViewModels;

namespace MTCG.API.Controllers;

[ApiController]
[Admin]
public class AdminController : ControllerBase
{

    private readonly PackageService _packageService;

    public AdminController(PackageService packageService)
    {
        _packageService = packageService;
    }

    [Post("/packages")]
    public async ValueTask<ActionResult> CreatePackage([FromBody] CardPackageCreationDto cardPackageCreationDto,HttpContext context)
    {
        try
        {
            CardPackageViewModel cardPackageViewModel = await _packageService.CreatePackageAsync(cardPackageCreationDto, context.UserName!);
            return Created($"/packages/{cardPackageViewModel.CardPackageId}", cardPackageViewModel);
        }
        catch (UnexistingCardException)
        {
            return BadRequest("One or more cards in the package do not exist.");
        }
        catch (DuplicateCardGuidException)
        {
            return BadRequest("One or more cards in the package have the same id.");
        }
    }

}