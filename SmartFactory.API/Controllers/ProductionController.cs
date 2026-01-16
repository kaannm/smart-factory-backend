using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFactory.Application.Features.Analytics.Interfaces;
using SmartFactory.Application.Features.Production.DTOs;
using SmartFactory.Application.Features.Production.Interfaces;

namespace SmartFactory.API.Controllers;

/// <summary>
/// Üretim kayıtları yönetimi için controller
/// </summary>
[ApiController]
[Route("api/production-records")]
[Authorize]
public class ProductionController : ControllerBase
{
    private readonly IProductionService _service;
    private readonly IProductionQueryService _queryService;

    public ProductionController(
        IProductionService service,
        IProductionQueryService queryService)
    {
        _service = service;
        _queryService = queryService;
    }

    /// <summary>
    /// Yeni üretim kaydı oluşturur
    /// </summary>
    /// <param name="dto">Üretim kaydı bilgileri</param>
    /// <returns>Başarı mesajı</returns>
    /// <response code="200">Başarılı - Kayıt oluşturuldu</response>
    /// <response code="400">Geçersiz veri</response>
    /// <response code="401">Yetkisiz erişim - Engineer veya Admin gerekli</response>
    [HttpPost]
    [Authorize(Policy = "EngineerOrAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateRecord([FromBody] CreateProductionRecordDto dto)
    {
        await _service.AddRecordAsync(dto);
        return Ok();
    }

    /// <summary>
    /// Üretim kayıtlarını filtreleyerek listeler
    /// </summary>
    /// <param name="lineId">Üretim hattı ID'si (opsiyonel)</param>
    /// <param name="from">Başlangıç tarihi (opsiyonel)</param>
    /// <param name="to">Bitiş tarihi (opsiyonel)</param>
    /// <returns>Filtrelenmiş üretim kayıtları listesi</returns>
    /// <response code="200">Başarılı - Kayıtlar listesi</response>
    /// <response code="401">Yetkisiz erişim</response>
    [HttpGet]
    [Authorize(Policy = "ViewerOrAbove")]
    [ProducesResponseType(typeof(List<ProductionRecordDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetRecords(
        [FromQuery] int? lineId, 
        [FromQuery] DateTime? from, 
        [FromQuery] DateTime? to)
    {
        var result = await _queryService.GetProductionRecordsAsync(lineId, from, to);
        return Ok(result);
    }
}