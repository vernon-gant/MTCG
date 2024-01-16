namespace MTCG.Services.UserService;

public interface SecurityService
{

    bool VerifyPassword(string hashedPassword, string rawPassword);

    string HashPassword(string rawPassword);

    string GenerateToken(string username, string role);

}