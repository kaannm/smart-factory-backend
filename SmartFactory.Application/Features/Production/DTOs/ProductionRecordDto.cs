namespace SmartFactory.Application.Features.Production.DTOs;

public class ProductionRecordDto
{
    public int Id { get; set; }
    public int ProductionLineId { get; set; }
    public string ProductionLineName { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public int ProducedCount { get; set; }
    public int FaultCount { get; set; }
}

