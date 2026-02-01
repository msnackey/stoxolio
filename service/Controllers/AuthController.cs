using Microsoft.AspNetCore.Mvc;
using Stoxolio.Service.DTOs;
using Stoxolio.Service.Services;

namespace Stoxolio.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, message, token) = await _authService.RegisterAsync(request.Username, request.Email, request.Password);

        var response = new AuthResponse { Success = success, Message = message, Token = token };

        return success ? Ok(response) : BadRequest(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, message, token) = await _authService.LoginAsync(request.Username, request.Password);

        var response = new AuthResponse { Success = success, Message = message, Token = token };

        return success ? Ok(response) : Unauthorized(response);
    }
}
