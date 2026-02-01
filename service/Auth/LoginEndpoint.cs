using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.DTOs;

namespace Stoxolio.Service.Auth;

public sealed record LoginCommand(LoginRequest Request) : ICommand<AuthResponse>;

public class LoginHandler(IAuthService authService) : ICommandHandler<LoginCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var (success, message, token) = await authService.LoginAsync(command.Request.Username, command.Request.Password);
        return new AuthResponse { Success = success, Message = message, Token = token };
    }
}