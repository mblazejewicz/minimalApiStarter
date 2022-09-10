using Microsoft.AspNetCore.Authorization;

public static class AuthEndpoints
{
    public static void UseAuthEndpoints(this WebApplication app)
    {
        app.MapPost(
        "/security/getToken",

        [AllowAnonymous]
        (IJwtTokenService _jwtService, LoginQuery user) =>
        {

            if (ValidateUser(user))
            {
                var data = GetUserData(user.UserName);
                var jwtToken = _jwtService.GenerateToken(data);
                return Results.Ok(jwtToken);
            }
            else
            {
                return Results.Unauthorized();
            }
        });
    }

    private static UserCredentials GetUserData(string userName)
    {
        return new UserCredentials(Guid.NewGuid(), userName, "admin");
    }

    private static bool ValidateUser(LoginQuery user)
    {
        return user.UserName == "admin@test.com" && user.Password == "Pass";
    }
}