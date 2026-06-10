using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stoxolio.Service.Data;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Auth;

public interface IAuthService
{
    Task<(bool success, string message, string? accessToken, string? refreshToken)> RegisterAsync(string username,
        string email, string password);

    Task<(bool success, string message, string? accessToken, string? refreshToken)> LoginAsync(string username,
        string password);

    Task<(bool success, string? accessToken, string? newRefreshToken)> RefreshAsync(string refreshToken);
    Task RevokeRefreshTokenAsync(string refreshToken);
}

public class AuthService(StoxolioDbContext context, IConfiguration configuration) : IAuthService
{
    private const int AccessTokenExpiryMinutes = 5;
    private const int RefreshTokenExpiryDays = 7;

    public async Task<(bool success, string message, string? accessToken, string? refreshToken)> RegisterAsync(
        string username, string email, string password)
    {
        if (await context.Users.AnyAsync(u => u.Username == username))
            return (false, "Username already exists", null, null);

        if (await context.Users.AnyAsync(u => u.Email == email))
            return (false, "Email already exists", null, null);

        var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);

        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        var accessToken = GenerateJwtToken(user);
        var refreshToken = await CreateRefreshTokenAsync(user.Id);

        return (true, "Registration successful", accessToken, refreshToken);
    }

    public async Task<(bool success, string message, string? accessToken, string? refreshToken)> LoginAsync(
        string username, string password)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(password, user.PasswordHash))
            return (false, "Invalid credentials", null, null);

        var accessToken = GenerateJwtToken(user);
        var refreshToken = await CreateRefreshTokenAsync(user.Id);

        return (true, "Login successful", accessToken, refreshToken);
    }

    public async Task<(bool success, string? accessToken, string? newRefreshToken)> RefreshAsync(string refreshToken)
    {
        var existing = await context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == refreshToken);

        if (existing == null || existing.IsRevoked || existing.Expires < DateTime.UtcNow)
            return (false, null, null);

        // Rotate: revoke old, issue new
        existing.IsRevoked = true;
        var newAccessToken = GenerateJwtToken(existing.User!);
        var newRefreshToken = await CreateRefreshTokenAsync(existing.UserId);

        return (true, newAccessToken, newRefreshToken);
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        var token = await context.RefreshTokens
            .FirstOrDefaultAsync(r => r.Token == refreshToken && !r.IsRevoked);

        if (token != null)
        {
            token.IsRevoked = true;
            await context.SaveChangesAsync();
        }
    }

    private async Task<string> CreateRefreshTokenAsync(long userId)
    {
        var tokenValue = GenerateRefreshTokenValue();
        context.RefreshTokens.Add(new RefreshToken
        {
            UserId = userId,
            Token = tokenValue,
            Expires = DateTime.UtcNow.AddDays(RefreshTokenExpiryDays),
            CreatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();
        return tokenValue;
    }

    private static string GenerateRefreshTokenValue()
    {
        var bytes = new byte[64];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(AccessTokenExpiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
