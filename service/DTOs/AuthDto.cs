namespace Stoxolio.Service.DTOs;

public class LoginRequest
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public class RegisterRequest
{
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public class AuthResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public string? Token { get; init; }
}
