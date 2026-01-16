using SmartFactory.Domain.Enums;

namespace SmartFactory.Domain.Entities;

public class ProductionLine : BaseEntity
{
    public string Name { get; set; } = null!;
    public ProductionLineStatus Status { get; set; }
}

