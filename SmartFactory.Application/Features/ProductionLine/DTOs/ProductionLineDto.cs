using SmartFactory.Domain.Enums;

namespace SmartFactory.Application.Features.ProductionLine.DTOs;

public class ProductionLineDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ProductionLineStatus Status { get; set; }
}

