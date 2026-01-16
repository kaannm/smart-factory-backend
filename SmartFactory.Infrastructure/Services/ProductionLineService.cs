using Microsoft.EntityFrameworkCore;
using SmartFactory.Application.Features.ProductionLine.DTOs;
using SmartFactory.Application.Features.ProductionLine.Interfaces;
using SmartFactory.Domain.Entities;
using SmartFactory.Infrastructure.Data;

namespace SmartFactory.Infrastructure.Services;

public class ProductionLineService : IProductionLineService
{
    private readonly AppDbContext _context;

    public ProductionLineService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductionLineDto>> GetAllAsync()
    {
        return await _context.ProductionLines
            .Select(pl => new ProductionLineDto
            {
                Id = pl.Id,
                Name = pl.Name,
                Status = pl.Status
            })
            .ToListAsync();
    }

    public async Task<ProductionLineDto> CreateAsync(CreateProductionLineDto dto)
    {
        var productionLine = new ProductionLine
        {
            Name = dto.Name,
            Status = dto.Status
        };

        _context.ProductionLines.Add(productionLine);
        await _context.SaveChangesAsync();

        return new ProductionLineDto
        {
            Id = productionLine.Id,
            Name = productionLine.Name,
            Status = productionLine.Status
        };
    }
}

