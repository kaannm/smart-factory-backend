using Microsoft.EntityFrameworkCore;
using SmartFactory.Application.Features.Production.DTOs;
using SmartFactory.Application.Features.Production.Interfaces;
using SmartFactory.Domain.Entities;
using SmartFactory.Infrastructure.Data;

namespace SmartFactory.Infrastructure.Services;

public class ProductionService : IProductionService
{
    private readonly AppDbContext _context;

    public ProductionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddRecordAsync(CreateProductionRecordDto dto)
    {
        var record = new ProductionRecord
        {
            ProductionLineId = dto.ProductionLineId,
            Timestamp = dto.Timestamp,
            ProducedCount = dto.ProducedCount,
            FaultCount = dto.FaultCount
        };

        _context.ProductionRecords.Add(record);
        await _context.SaveChangesAsync();
    }
}

