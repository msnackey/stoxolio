using Stoxolio.Service.Auth;
using Stoxolio.Service.BuildingBlocks.CQRS;
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
                ICommandHandler<LoginCommand, AuthResponse> handler,
                AuthService authService,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new LoginCommand(request), cancellationToken);

                return Results.Ok(result);

                // TODO: Implement Results class
                // return result.IsSuccess
                //     ? Results.Ok(result.Value)
                //     : ApiResults.Problem(result);
            })
            .WithName("Login")
            .WithDescription("User login endpoint.")
            .Produces<AuthResponse>(200)
            .ProducesProblem(400)
            .ProducesProblem(500)
            .ProducesProblem(502);

        group.MapPost("/register", async (
                RegisterRequest request,
                ICommandHandler<RegisterCommand, AuthResponse> handler,
                AuthService authService,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new RegisterCommand(request), cancellationToken);
                
                return Results.Ok(result);

                // TODO: Implement Results class
                // return result.IsSuccess
                //     ? Results.Ok(result.Value)
                //     : ApiResults.Problem(result);
            })
            .WithName("Register")
            .WithDescription("User registration endpoint.")
            .Produces<AuthResponse>(200)
            .ProducesProblem(400)
            .ProducesProblem(500)
            .ProducesProblem(502);
    }
}
