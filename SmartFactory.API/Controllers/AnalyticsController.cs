using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFactory.Application.Features.Analytics.DTOs;
using SmartFactory.Application.Features.Analytics.Interfaces;

namespace SmartFactory.API.Controllers;

/// <summary>
/// Analitik ve raporlama işlemleri için controller
/// </summary>
[ApiController]
[Route("api/analytics")]
[Authorize]
public class AnalyticsController : ControllerBase
{
    private readonly IProductionQueryService _queryService;

    public AnalyticsController(IProductionQueryService queryService)
    {
        _queryService = queryService;
    }

    /// <summary>
    /// Günlük üretim özeti getirir (Digital Transformation / Analytics için)
    /// </summary>
    /// <returns>Günlük toplam üretim, hata sayısı ve verimlilik oranı</returns>
    /// <response code="200">Başarılı - Günlük özet verileri</response>
    /// <response code="401">Yetkisiz erişim</response>
    /// <remarks>
    /// Örnek çıktı:
    /// {
    ///   "totalProduced": 1240,
    ///   "totalFaults": 23,
    ///   "efficiency": 98.1
    /// }
    /// </remarks>
    [HttpGet("daily-summary")]
    [Authorize(Policy = "ViewerOrAbove")]
    [ProducesResponseType(typeof(DailySummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetDailySummary()
    {
        var result = await _queryService.GetDailySummaryAsync();
        return Ok(result);
    }
}

