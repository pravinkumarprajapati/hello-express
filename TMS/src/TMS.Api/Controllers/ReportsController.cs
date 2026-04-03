using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Contracts;
using TMS.Application.DTOs;

namespace TMS.Api.Controllers;

[ApiController]
[Authorize(Policy = "ManagerPolicy")]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("utilization")]
    [ProducesResponseType(typeof(IReadOnlyCollection<UtilizationReportRowDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUtilization([FromQuery] string? name, [FromQuery] string? department, [FromQuery] DateTime? fromOnUtc, [FromQuery] DateTime? toOnUtc, CancellationToken cancellationToken)
    {
        var filter = new UtilizationReportFilterDto
        {
            Name = name,
            Department = department,
            FromOnUtc = fromOnUtc,
            ToOnUtc = toOnUtc
        };

        var report = await _reportService.GetUtilizationReportAsync(filter, cancellationToken);
        return Ok(report);
    }

    [HttpGet("utilization/export/excel")]
    public async Task<IActionResult> ExportUtilizationExcel([FromQuery] string? name, [FromQuery] string? department, [FromQuery] DateTime? fromOnUtc, [FromQuery] DateTime? toOnUtc, CancellationToken cancellationToken)
    {
        var filter = new UtilizationReportFilterDto
        {
            Name = name,
            Department = department,
            FromOnUtc = fromOnUtc,
            ToOnUtc = toOnUtc
        };

        byte[] content = await _reportService.ExportUtilizationExcelAsync(filter, cancellationToken);
        const string fileName = "instructor-utilization.csv";
        return File(content, "application/vnd.ms-excel", fileName);
    }
}
