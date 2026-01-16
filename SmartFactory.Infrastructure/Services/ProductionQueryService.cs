using Microsoft.EntityFrameworkCore;
using SmartFactory.Application.Features.Analytics.DTOs;
using SmartFactory.Application.Features.Analytics.Interfaces;
using SmartFactory.Application.Features.Production.DTOs;
using SmartFactory.Infrastructure.Data;

namespace SmartFactory.Infrastructure.Services;

public class ProductionQueryService : IProductionQueryService
{
    private readonly AppDbContext _context;

    public ProductionQueryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DailySummaryDto> GetDailySummaryAsync()
    {
        var today = DateTime.UtcNow.Date;

        var summary = await _context.ProductionRecords
            .Where(x => x.Timestamp.Date == today)
            .GroupBy(x => 1)
            .Select(g => new
            {
                TotalProduced = g.Sum(x => x.ProducedCount),
                TotalFaults = g.Sum(x => x.FaultCount)
            })
            .FirstOrDefaultAsync();

        if (summary == null)
        {
            return new DailySummaryDto
            {
                TotalProduced = 0,
                TotalFaults = 0,
                Efficiency = 100.0
            };
        }

        var totalProduced = summary.TotalProduced;
        var totalFaults = summary.TotalFaults;
        
        var efficiency = totalProduced > 0 
            ? ((double)(totalProduced - totalFaults) / totalProduced) * 100.0 
            : 100.0;

        return new DailySummaryDto
        {
            TotalProduced = totalProduced,
            TotalFaults = totalFaults,
            Efficiency = Math.Round(efficiency, 1)
        };
    }

    public async Task<List<ProductionRecordDto>> GetProductionRecordsAsync(int? lineId, DateTime? from, DateTime? to)
    {
        var query = _context.ProductionRecords
            .Include(pr => pr.ProductionLine)
            .AsQueryable();

        if (lineId.HasValue)
        {
            query = query.Where(pr => pr.ProductionLineId == lineId.Value);
        }

        if (from.HasValue)
        {
            query = query.Where(pr => pr.Timestamp >= from.Value);
        }

        if (to.HasValue)
        {
            query = query.Where(pr => pr.Timestamp <= to.Value);
        }

        return await query
            .OrderByDescending(pr => pr.Timestamp)
            .Select(pr => new ProductionRecordDto
            {
                Id = pr.Id,
                ProductionLineId = pr.ProductionLineId,
                ProductionLineName = pr.ProductionLine.Name,
                Timestamp = pr.Timestamp,
                ProducedCount = pr.ProducedCount,
                FaultCount = pr.FaultCount
            })
            .ToListAsync();
    }
}

