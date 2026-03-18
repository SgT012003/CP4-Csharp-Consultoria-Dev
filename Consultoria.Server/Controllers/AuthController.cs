using Consultoria.Infrastructure.Entities;
using Consultoria.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Consultoria.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return Unauthorized(new AuthResponseDto { Success = false, Message = "Credenciais inválidas." });

        var result = await _signInManager.PasswordSignInAsync(user, dto.Password, isPersistent: false, lockoutOnFailure: true);
        if (!result.Succeeded)
            return Unauthorized(new AuthResponseDto { Success = false, Message = "Credenciais inválidas." });

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(new AuthResponseDto
        {
            Success = true,
            Message = "Login realizado com sucesso.",
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                Roles = roles
            }
        });
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest(new AuthResponseDto { Success = false, Message = errors });
        }

        await _userManager.AddToRoleAsync(user, "Client");
        await _signInManager.SignInAsync(user, isPersistent: false);

        return Ok(new AuthResponseDto
        {
            Success = true,
            Message = "Conta criada com sucesso.",
            User = new UserDto { Id = user.Id, Email = user.Email!, FullName = user.FullName, Roles = new[] { "Client" } }
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok(new { message = "Logout realizado." });
    }

    [HttpGet("user")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        if (!User.Identity?.IsAuthenticated ?? true)
            return Unauthorized();

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            FullName = user.FullName,
            Roles = roles
        });
    }
}
