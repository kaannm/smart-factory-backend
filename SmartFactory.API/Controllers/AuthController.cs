using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFactory.Application.Features.Auth.DTOs;
using SmartFactory.Application.Features.Auth.Interfaces;

namespace SmartFactory.API.Controllers;

/// <summary>
/// Authentication işlemleri için controller
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Kullanıcı girişi yapar ve JWT token döner
    /// </summary>
    /// <param name="loginDto">Email ve şifre bilgileri</param>
    /// <returns>JWT token ve kullanıcı bilgileri</returns>
    /// <response code="200">Başarılı giriş - Token döner</response>
    /// <response code="401">Email veya şifre hatalı</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var result = await _authService.LoginAsync(loginDto);

            if (result == null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Yeni kullanıcı kaydı oluşturur
    /// </summary>
    /// <param name="registerDto">Kullanıcı bilgileri</param>
    /// <returns>Oluşturulan kullanıcı bilgileri</returns>
    /// <response code="201">Başarılı - Kullanıcı oluşturuldu</response>
    /// <response code="400">Email zaten kullanılıyor veya geçersiz veri</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            var result = await _authService.RegisterAsync(registerDto);
            return CreatedAtAction(nameof(Login), new { email = result.Email }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Onay bekleyen kullanıcıları listeler (Admin only)
    /// </summary>
    /// <returns>Onay bekleyen kullanıcılar listesi</returns>
    /// <response code="200">Başarılı - Kullanıcılar listesi</response>
    /// <response code="401">Yetkisiz erişim - Sadece Admin</response>
    [HttpGet("pending-users")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetPendingUsers()
    {
        var result = await _authService.GetPendingUsersAsync();
        return Ok(result);
    }

    /// <summary>
    /// Kullanıcıyı onaylar veya reddeder (Admin only)
    /// </summary>
    /// <param name="userId">Onaylanacak kullanıcı ID'si</param>
    /// <param name="isApproved">Onay durumu (true/false)</param>
    /// <returns>Güncellenmiş kullanıcı bilgileri</returns>
    /// <response code="200">Başarılı - Kullanıcı onaylandı/reddedildi</response>
    /// <response code="404">Kullanıcı bulunamadı</response>
    /// <response code="401">Yetkisiz erişim - Sadece Admin</response>
    [HttpPut("approve-user/{userId}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ApproveUser(int userId, [FromQuery] bool isApproved = true)
    {
        try
        {
            var result = await _authService.ApproveUserAsync(userId, isApproved);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}

