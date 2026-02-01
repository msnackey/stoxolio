using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stoxolio.Service.Data;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Auth;

public interface IAuthService
{
    Task<(bool success, string message, string? token)> RegisterAsync(string username, string email, string password);
    Task<(bool success, string message, string? token)> LoginAsync(string username, string password);
}

public class AuthService : IAuthService
{
    private readonly StoxolioDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(StoxolioDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<(bool success, string message, string? token)> RegisterAsync(string username, string email, string password)
    {
        // Check if user already exists
        if (await _context.Users.AnyAsync(u => u.Username == username))
            return (false, "Username already exists", null);

        if (await _context.Users.AnyAsync(u => u.Email == email))
            return (false, "Email already exists", null);

        // Hash password
        var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);

        // Create user
        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Generate token
        var token = GenerateJwtToken(user);

        return (true, "Registration successful", token);
    }

    public async Task<(bool success, string message, string? token)> LoginAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
            return (false, "Invalid credentials", null);

        // Verify password
        if (!BCrypt.Net.BCrypt.EnhancedVerify(password, user.PasswordHash))
            return (false, "Invalid credentials", null);

        // Generate token
        var token = GenerateJwtToken(user);

        return (true, "Login successful", token);
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
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
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
