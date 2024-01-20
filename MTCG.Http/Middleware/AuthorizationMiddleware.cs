using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using MTCG.API.attributes;

namespace MCTG.middleware;

public class AuthorizationMiddleware : Middleware
{

    private readonly UserSecrets _userSecrets;

    public AuthorizationMiddleware(Middleware? next, UserSecrets userSecrets) : base(next)
    {
        _userSecrets = userSecrets;
    }

    protected override Task Handle(HttpContext context)
    {
        AuthAttribute? authAttribute = context.Controller.GetCustomAttribute<AuthAttribute>();

        if (authAttribute == null) return Task.CompletedTask;

        if (context.Request.Token == null) throw new ShortCircuitException(new UnauthorizedResult("Unauthorized!"));

        if (!IsValidToken(context.Request.Token)) throw new ShortCircuitException(new UnauthorizedResult("Invalid token!"));

        if (TokenExpired(context.Request.Token)) throw new ShortCircuitException(new UnauthorizedResult("Token expired!"));

        if (authAttribute is AdminAttribute && !AdminInToken(context.Request.Token)) throw new ShortCircuitException(new ForbiddenResult("Forbidden!"));

        context.UserName = context.Request.Token?.Payload["username"].ToString();
        context.IsAdmin = AdminInToken(context.Request.Token!);

        return Task.CompletedTask;
    }

    private bool AdminInToken(JwtSecurityToken token) => token.Payload["role"]?.ToString() == "admin";

    private bool TokenExpired(JwtSecurityToken token) => token.ValidTo < DateTime.UtcNow;

    private bool IsValidToken(JwtSecurityToken token)
    {
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_userSecrets.JWTSecret));

        TokenValidationParameters validationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey
        };

        try
        {
            JwtSecurityTokenHandler tokenHandler = new ();
            tokenHandler.ValidateToken(token.RawData, validationParameters, out SecurityToken validatedToken);

            return IsValidTokenPayload(token);
        }
        catch
        {
            return false;
        }
    }

    private bool IsValidTokenPayload(JwtSecurityToken token)
    {
        if (token.Payload["username"] == null) return false;
        if (token.Payload["role"] == null) return false;
        if (token.Payload["exp"] == null) return false;

        return true;
    }

}

public class UserSecrets
{

    public string JWTSecret { get; set; }

}