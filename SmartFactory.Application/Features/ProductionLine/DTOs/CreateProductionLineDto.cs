using SmartFactory.Domain.Enums;

namespace SmartFactory.Application.Features.ProductionLine.DTOs;

public class CreateProductionLineDto
{
    public string Name { get; set; } = null!;
    public ProductionLineStatus Status { get; set; }
}

