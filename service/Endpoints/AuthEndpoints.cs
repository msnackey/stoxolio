using Stoxolio.Service.Auth;
using Stoxolio.Service.DTOs;
using Stoxolio.Service.Extensions;

namespace Stoxolio.Service.Endpoints;

public static class AuthEndpoints
{
    private const string AccessTokenCookie = "auth_token";
    private const string RefreshTokenCookie = "refresh_token";
    private const int AccessTokenExpiryMinutes = 15;
    private const int RefreshTokenExpiryDays = 7;

    private static CookieOptions CreateAccessTokenCookieOptions() => new()
    {
        HttpOnly = true,
        Secure = false, // set true in production
        SameSite = SameSiteMode.Lax,
        Expires = DateTimeOffset.UtcNow.AddMinutes(AccessTokenExpiryMinutes),
        Path = "/"
    };

    private static CookieOptions CreateRefreshTokenCookieOptions() => new()
    {
        HttpOnly = true,
        Secure = false, // set true in production
        SameSite = SameSiteMode.Lax,
        Expires = DateTimeOffset.UtcNow.AddDays(RefreshTokenExpiryDays),
        Path = "/api/auth" // only sent to auth endpoints
    };

    private static void SetAuthCookies(HttpContext httpContext, string accessToken, string refreshToken)
    {
        httpContext.Response.Cookies.Append(AccessTokenCookie, accessToken, CreateAccessTokenCookieOptions());
        httpContext.Response.Cookies.Append(RefreshTokenCookie, refreshToken, CreateRefreshTokenCookieOptions());
    }

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
                var (success, message, accessToken, refreshToken) =
                    await authService.LoginAsync(request.Username, request.Password);
                if (!success)
                    return Results.Unauthorized();
                SetAuthCookies(httpContext, accessToken!, refreshToken!);
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
                var (success, message, accessToken, refreshToken) =
                    await authService.RegisterAsync(request.Username, request.Email, request.Password);
                if (!success)
                    return Results.Conflict(new AuthResponse { Success = false, Message = message });
                SetAuthCookies(httpContext, accessToken!, refreshToken!);
                return Results.Ok(new AuthResponse { Success = true, Message = message, Username = request.Username });
            })
            .WithName("Register")
            .WithDescription("User registration endpoint.")
            .Produces<AuthResponse>(200)
            .ProducesProblem(409)
            .ProducesProblem(500)
            .ProducesProblem(502);

        group.MapPost("/refresh", async (
                IAuthService authService,
                HttpContext httpContext) =>
            {
                var refreshToken = httpContext.Request.Cookies[RefreshTokenCookie];
                if (string.IsNullOrEmpty(refreshToken))
                    return Results.Unauthorized();

                var (success, accessToken, newRefreshToken) = await authService.RefreshAsync(refreshToken);
                if (!success)
                    return Results.Unauthorized();

                SetAuthCookies(httpContext, accessToken!, newRefreshToken!);
                return Results.Ok();
            })
            .WithName("Refresh")
            .WithDescription("Refreshes the access token using a valid refresh token cookie.")
            .Produces(200)
            .ProducesProblem(401);

        group.MapPost("/logout", async (
                IAuthService authService,
                HttpContext httpContext) =>
            {
                var refreshToken = httpContext.Request.Cookies[RefreshTokenCookie];
                if (!string.IsNullOrEmpty(refreshToken))
                    await authService.RevokeRefreshTokenAsync(refreshToken);

                httpContext.Response.Cookies.Delete(AccessTokenCookie, CreateAccessTokenCookieOptions());
                httpContext.Response.Cookies.Delete(RefreshTokenCookie, CreateRefreshTokenCookieOptions());
                return Results.Ok();
            })
            .WithName("Logout")
            .WithDescription("Revokes the refresh token and clears auth cookies.")
            .Produces(200);
    }
}
