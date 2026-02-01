using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.DTOs;

namespace Stoxolio.Service.Auth;

public sealed record LoginQuery(string Username, string Password) : ICommand<AuthResponse>;

public class LoginEndpoint
{
    private static async Task<IResult> HandleLogin(
        LoginRequest request,
        IAuthService authService,
        CancellationToken cancellationToken)
    {
        var query = new LoginQuery(request.Username, request.Password);
        var handler = new LoginHandler(authService);
        var result = await handler.Handle(query, cancellationToken);

        return result.Success ? Results.Ok(result) : Results.Unauthorized();
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPost("/login", HandleLogin)
            .WithName("Login");
    }

    private class LoginHandler : ICommandHandler<LoginQuery, AuthResponse>
    {
        private readonly IAuthService _authService;

        public LoginHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var (success, message, token) = await _authService.LoginAsync(request.Username, request.Password);
            return new AuthResponse { Success = success, Message = message, Token = token };
        }
    }
}
