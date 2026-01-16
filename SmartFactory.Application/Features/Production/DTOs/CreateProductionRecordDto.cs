namespace SmartFactory.Application.Features.Production.DTOs;

public class CreateProductionRecordDto
{
    public int ProductionLineId { get; set; }
    public DateTime Timestamp { get; set; }
    public int ProducedCount { get; set; }
    public int FaultCount { get; set; }
}

