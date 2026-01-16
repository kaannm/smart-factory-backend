using SmartFactory.Domain.Entities;

namespace SmartFactory.Application.Features.Auth.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}

