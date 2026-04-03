using Microsoft.AspNetCore.Mvc;
using TMS.Application.Contracts;
using TMS.Application.DTOs;

namespace TMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SyncController : ControllerBase
{
    private readonly ILeaveSyncService _leaveSyncService;

    public SyncController(ILeaveSyncService leaveSyncService)
    {
        _leaveSyncService = leaveSyncService;
    }

    [HttpPost("leaves")]
    [ProducesResponseType(typeof(LeaveSyncResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> SyncLeaves(CancellationToken cancellationToken)
    {
        LeaveSyncResultDto result = await _leaveSyncService.SyncLeavesAsync(cancellationToken);
        return Ok(result);
    }
}
