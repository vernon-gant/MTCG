using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

using MCTG.middleware;

using Microsoft.IdentityModel.Tokens;

namespace MTCG.Services.UserService;

public class DefaultSecurityService : SecurityService
{

    private readonly UserSecrets _userSecrets;

    private const int SaltSize = 128;

    public DefaultSecurityService(UserSecrets userSecrets)
    {
        _userSecrets = userSecrets;
    }

    public bool VerifyPassword(string hashedPassword, string rawPassword)
    {
        byte[] hashBytes = Convert.FromBase64String(hashedPassword);
        byte[] salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, salt.Length);

        using HMACSHA512 hmac = new HMACSHA512(salt);
        byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawPassword));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != hashBytes[i + salt.Length]) return false;
        }

        return true;
    }


    public string HashPassword(string rawPassword)
    {
        using HMACSHA512 hmac = new ();

        byte[] salt = hmac.Key;
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawPassword));
        byte[] hashBytes = new byte[salt.Length + hash.Length];
        Array.Copy(salt, 0, hashBytes, 0, salt.Length);
        Array.Copy(hash, 0, hashBytes, salt.Length, hash.Length);

        return Convert.ToBase64String(hashBytes);
    }

    public string GenerateToken(string username, string role)
    {
        JwtSecurityTokenHandler tokenHandler = new ();
        byte[] key = Encoding.ASCII.GetBytes(_userSecrets.JWTSecret);

        SecurityTokenDescriptor tokenDescriptor = new ()
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new[]
            {
                new System.Security.Claims.Claim("username", username),
                new System.Security.Claims.Claim("role", role),
            }),
            Expires = DateTime.UtcNow.AddHours(2), // Set token expiration as needed
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

}