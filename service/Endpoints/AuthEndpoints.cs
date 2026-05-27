using Stoxolio.Service.Auth;
using Stoxolio.Service.DTOs;
using Stoxolio.Service.Extensions;

namespace Stoxolio.Service.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
            .WithName("Auth");

        group.MapPost("/login", async (
                LoginRequest request,
                IAuthService authService,
                CancellationToken cancellationToken) =>
            {
                var (success, message, token) = await authService.LoginAsync(request.Username, request.Password);
                if (!success)
                    return Results.Unauthorized();
                return Results.Ok(new AuthResponse { Success = true, Message = message, Token = token });
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
                CancellationToken cancellationToken) =>
            {
                var (success, message, token) =
                    await authService.RegisterAsync(request.Username, request.Email, request.Password);
                if (!success)
                    return Results.Conflict(new AuthResponse { Success = false, Message = message });
                return Results.Ok(new AuthResponse { Success = true, Message = message, Token = token });
            })
            .WithName("Register")
            .WithDescription("User registration endpoint.")
            .Produces<AuthResponse>(200)
            .ProducesProblem(409)
            .ProducesProblem(500)
            .ProducesProblem(502);
    }
}
