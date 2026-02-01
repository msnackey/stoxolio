using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.DTOs;

namespace Stoxolio.Service.Auth;

public sealed record RegisterCommand(RegisterRequest Request) : ICommand<AuthResponse>;

public class RegisterHandler(IAuthService authService) : ICommandHandler<RegisterCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var (success, message, token) = await authService.RegisterAsync(command.Request.Username, command.Request.Email, command.Request.Password);
        return new AuthResponse { Success = success, Message = message, Token = token };
    }
}