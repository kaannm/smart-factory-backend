namespace SmartFactory.Domain.Entities;

public class ProductionRecord : BaseEntity
{
    public int ProductionLineId { get; set; }
    public ProductionLine ProductionLine { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public int ProducedCount { get; set; }
    public int FaultCount { get; set; }
}

