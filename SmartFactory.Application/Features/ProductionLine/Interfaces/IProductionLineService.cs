using SmartFactory.Application.Features.ProductionLine.DTOs;

namespace SmartFactory.Application.Features.ProductionLine.Interfaces;

public interface IProductionLineService
{
    Task<List<ProductionLineDto>> GetAllAsync();
    Task<ProductionLineDto> CreateAsync(CreateProductionLineDto dto);
}

