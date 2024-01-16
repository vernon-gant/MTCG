using MCTG;

namespace MTCG.API.attributes;

public class ControllerBase
{

    protected OkResult Ok(object? value = default)
    {
        return new OkResult(value);
    }

    protected CreatedResult Created(string location, object? value = default)
    {
        return new CreatedResult(location, value);
    }

    protected BadRequestResult BadRequest(string? message = default)
    {
        return new BadRequestResult(message);
    }

    protected ConflictResult Conflict(string? message = default)
    {
        return new ConflictResult(message);
    }

    protected NotFoundResult NotFound(string? message = default)
    {
        return new NotFoundResult(message);
    }

    protected UnauthorizedResult Unauthorized(string? message = default)
    {
        return new UnauthorizedResult(message);
    }

}