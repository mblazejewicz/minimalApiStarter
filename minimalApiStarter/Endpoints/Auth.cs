using Microsoft.AspNetCore.Authorization;

namespace minimalApiStarter.Endpoints;

public static class AuthEndpoints
{
    public static void UseAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/security/getToken", AuthEndpointHandler);
    }

    [AllowAnonymous]
    private static async Task<IResult> AuthEndpointHandler(HttpContext context,
        IJwtTokenService _jwtService, LoginQuery user, CancellationToken cancellationToken)
    {
        if (await ValidateUser(user))
        {
            var data = GetUserData(user.UserName);
            var jwtToken = _jwtService.GenerateToken(data);
            return Results.Ok(jwtToken);
        }

        return Results.BadRequest( "Incorrect Login or password");
    }

    private static UserCredentials GetUserData(string userName)
    {
        return new UserCredentials(Guid.NewGuid(), userName, "admin");
    }

    private static async Task<bool> ValidateUser(LoginQuery user)
    {
        await Task.Delay(5);
        return user.UserName == "admin@test.com" && user.Password == "Pass";
    }
}