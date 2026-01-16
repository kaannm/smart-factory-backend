using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFactory.Application.Features.ProductionLine.DTOs;
using SmartFactory.Application.Features.ProductionLine.Interfaces;

namespace SmartFactory.API.Controllers;

/// <summary>
/// Üretim hatları yönetimi için controller
/// </summary>
[ApiController]
[Route("api/production-lines")]
[Authorize]
public class ProductionLineController : ControllerBase
{
    private readonly IProductionLineService _service;

    public ProductionLineController(IProductionLineService service)
    {
        _service = service;
    }

    /// <summary>
    /// Tüm üretim hatlarını listeler
    /// </summary>
    /// <returns>Üretim hatları listesi</returns>
    /// <response code="200">Başarılı - Üretim hatları listesi</response>
    /// <response code="401">Yetkisiz erişim</response>
    [HttpGet]
    [Authorize(Policy = "ViewerOrAbove")]
    [ProducesResponseType(typeof(List<ProductionLineDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    /// <summary>
    /// Yeni üretim hattı oluşturur
    /// </summary>
    /// <param name="dto">Üretim hattı bilgileri</param>
    /// <returns>Oluşturulan üretim hattı</returns>
    /// <response code="201">Başarılı - Üretim hattı oluşturuldu</response>
    /// <response code="400">Geçersiz veri</response>
    /// <response code="401">Yetkisiz erişim - Sadece Admin</response>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(ProductionLineDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateProductionLineDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }
}

