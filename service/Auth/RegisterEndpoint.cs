using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.DTOs;

namespace Stoxolio.Service.Auth;

public sealed record RegisterQuery(string Username, string Email, string Password) : ICommand<AuthResponse>;

public class RegisterEndpoint
{
    private static async Task<IResult> HandleRegister(
        RegisterRequest request,
        IAuthService authService,
        CancellationToken cancellationToken)
    {
        var query = new RegisterQuery(request.Username, request.Email, request.Password);
        var handler = new RegisterHandler(authService);
        var result = await handler.Handle(query, cancellationToken);

        return result.Success ? Results.Ok(result) : Results.BadRequest(result);
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPost("/register", HandleRegister)
            .WithName("Register");
    }

    private class RegisterHandler : ICommandHandler<RegisterQuery, AuthResponse>
    {
        private readonly IAuthService _authService;

        public RegisterHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthResponse> Handle(RegisterQuery request, CancellationToken cancellationToken)
        {
            var (success, message, token) = await _authService.RegisterAsync(request.Username, request.Email, request.Password);
            return new AuthResponse { Success = success, Message = message, Token = token };
        }
    }
}
