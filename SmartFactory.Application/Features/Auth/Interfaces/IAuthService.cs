using SmartFactory.Application.Features.Auth.DTOs;

namespace SmartFactory.Application.Features.Auth.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
    Task<UserDto> RegisterAsync(RegisterDto registerDto);
    Task<List<UserDto>> GetPendingUsersAsync();
    Task<UserDto> ApproveUserAsync(int userId, bool isApproved);
}

