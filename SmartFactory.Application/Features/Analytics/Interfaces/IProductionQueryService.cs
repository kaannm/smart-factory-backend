using SmartFactory.Application.Features.Analytics.DTOs;
using SmartFactory.Application.Features.Production.DTOs;

namespace SmartFactory.Application.Features.Analytics.Interfaces;

public interface IProductionQueryService
{
    Task<DailySummaryDto> GetDailySummaryAsync();
    Task<List<ProductionRecordDto>> GetProductionRecordsAsync(int? lineId, DateTime? from, DateTime? to);
}

