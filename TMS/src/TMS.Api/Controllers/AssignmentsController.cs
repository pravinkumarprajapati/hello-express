using Microsoft.AspNetCore.Mvc;
using TMS.Application.Contracts;
using TMS.Application.DTOs;

namespace TMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssignmentsController : ControllerBase
{
    private readonly IAssignmentEngineService _assignmentEngineService;

    public AssignmentsController(IAssignmentEngineService assignmentEngineService)
    {
        _assignmentEngineService = assignmentEngineService;
    }

    [HttpPost("{sessionId:guid}/auto")]
    [ProducesResponseType(typeof(AutoAssignmentResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AutoAssign(Guid sessionId, CancellationToken cancellationToken)
    {
        AutoAssignmentResultDto result = await _assignmentEngineService.AutoAssignAsync(sessionId, cancellationToken);

        if (result.Status == "NotFound")
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}
