using Microsoft.AspNetCore.Mvc;
using F1Backend.Models;
using F1Backend.Services;

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
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
    {
        try
        {
            // Register the user using the AuthService
            var newUser = await _authService.RegisterAsync(registerDto);
            return Ok(newUser);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        try
        {
            // Authenticate the user and get the JWT token
            var token = await _authService.LoginAsync(loginDto);

            // Set the JWT token as a cookie
            Response.Cookies.Append("YourAppName.Auth", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(60)
            });

            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
