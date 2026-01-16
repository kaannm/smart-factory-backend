using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using SmartFactory.Application.Features.Auth.DTOs;
using SmartFactory.Application.Features.Auth.Interfaces;
using SmartFactory.Domain.Entities;
using SmartFactory.Domain.Enums;
using SmartFactory.Infrastructure.Data;

namespace SmartFactory.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthService(AppDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user == null)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            return null;

        if (!user.IsApproved)
            throw new UnauthorizedAccessException("Account is pending approval");

        var token = _jwtService.GenerateToken(user);

            return new LoginResponseDto
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                IsApproved = user.IsApproved
            }
        };
    }

    public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
    {
        if (registerDto.Role == UserRole.Admin)
            throw new InvalidOperationException("Admin role cannot be registered. Admin accounts must be created by existing admins.");

        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == registerDto.Email);

        if (existingUser != null)
            throw new InvalidOperationException("Email already exists");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var user = new User
        {
            FullName = registerDto.FullName,
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            Role = registerDto.Role,
            IsApproved = false
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.ToString(),
            IsApproved = user.IsApproved
        };
    }

    public async Task<List<UserDto>> GetPendingUsersAsync()
    {
        return await _context.Users
            .Where(u => !u.IsApproved)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Role = u.Role.ToString(),
                IsApproved = u.IsApproved
            })
            .ToListAsync();
    }

    public async Task<UserDto> ApproveUserAsync(int userId, bool isApproved)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        user.IsApproved = isApproved;
        await _context.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.ToString(),
            IsApproved = user.IsApproved
        };
    }
}

