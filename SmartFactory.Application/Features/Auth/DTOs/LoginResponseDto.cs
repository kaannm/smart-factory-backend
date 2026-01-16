namespace SmartFactory.Application.Features.Auth.DTOs;

public class LoginResponseDto
{
    public string Token { get; set; } = null!;
    public UserDto User { get; set; } = null!;
}

