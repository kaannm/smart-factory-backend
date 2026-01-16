using SmartFactory.Application.Features.Production.DTOs;

namespace SmartFactory.Application.Features.Production.Interfaces;

public interface IProductionService
{
    Task AddRecordAsync(CreateProductionRecordDto dto);
}

