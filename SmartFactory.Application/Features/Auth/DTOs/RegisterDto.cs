using SmartFactory.Domain.Enums;

namespace SmartFactory.Application.Features.Auth.DTOs;

public class RegisterDto
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public UserRole Role { get; set; } = UserRole.Viewer;
}

