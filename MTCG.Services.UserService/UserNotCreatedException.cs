namespace MTCG.Services.UserService;

public class UserNotCreatedException : Exception
{

    public UserNotCreatedException(string message) : base(message) { }

}