using Stoxolio.Service.Auth;
using Stoxolio.Service.DTOs;

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
                return Results.Ok(new AuthResponse { Success = success, Message = message, Token = token });
            })
            .WithName("Login")
            .WithDescription("User login endpoint.")
            .Produces<AuthResponse>(200)
            .ProducesProblem(400)
            .ProducesProblem(500)
            .ProducesProblem(502);

        group.MapPost("/register", async (
                RegisterRequest request,
                IAuthService authService,
                CancellationToken cancellationToken) =>
            {
                var (success, message, token) = await authService.RegisterAsync(request.Username, request.Email, request.Password);
                return Results.Ok(new AuthResponse { Success = success, Message = message, Token = token });
            })
            .WithName("Register")
            .WithDescription("User registration endpoint.")
            .Produces<AuthResponse>(200)
            .ProducesProblem(400)
            .ProducesProblem(500)
            .ProducesProblem(502);
    }
}
