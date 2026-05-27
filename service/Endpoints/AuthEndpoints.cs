using Stoxolio.Service.Auth;
using Stoxolio.Service.DTOs;
using Stoxolio.Service.Extensions;

namespace Stoxolio.Service.Endpoints;

public static class AuthEndpoints
{
    private const string CookieName = "auth_token";
    private const int CookieExpiryHours = 24;

    private static CookieOptions CreateAuthCookieOptions() => new()
    {
        HttpOnly = true,
        Secure = false, // set to true in production via env config
        SameSite = SameSiteMode.Lax,
        Expires = DateTimeOffset.UtcNow.AddHours(CookieExpiryHours),
        Path = "/"
    };

    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
            .WithName("Auth");

        group.MapPost("/login", async (
                LoginRequest request,
                IAuthService authService,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var (success, message, token) = await authService.LoginAsync(request.Username, request.Password);
                if (!success)
                    return Results.Unauthorized();
                httpContext.Response.Cookies.Append(CookieName, token!, CreateAuthCookieOptions());
                return Results.Ok(new AuthResponse { Success = true, Message = message, Username = request.Username });
            })
            .WithName("Login")
            .WithDescription("User login endpoint.")
            .RequireRateLimiting(DependencyInjection.LoginRateLimitPolicy)
            .Produces<AuthResponse>(200)
            .ProducesProblem(401)
            .ProducesProblem(429)
            .ProducesProblem(500)
            .ProducesProblem(502);

        group.MapPost("/register", async (
                RegisterRequest request,
                IAuthService authService,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var (success, message, token) =
                    await authService.RegisterAsync(request.Username, request.Email, request.Password);
                if (!success)
                    return Results.Conflict(new AuthResponse { Success = false, Message = message });
                httpContext.Response.Cookies.Append(CookieName, token!, CreateAuthCookieOptions());
                return Results.Ok(new AuthResponse { Success = true, Message = message, Username = request.Username });
            })
            .WithName("Register")
            .WithDescription("User registration endpoint.")
            .Produces<AuthResponse>(200)
            .ProducesProblem(409)
            .ProducesProblem(500)
            .ProducesProblem(502);

        group.MapPost("/logout", (HttpContext httpContext) =>
            {
                httpContext.Response.Cookies.Delete(CookieName, CreateAuthCookieOptions());
                return Results.Ok();
            })
            .WithName("Logout")
            .WithDescription("Clears the auth cookie.")
            .Produces(200);
    }
}
