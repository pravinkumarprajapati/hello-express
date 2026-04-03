using Microsoft.AspNetCore.Mvc;

namespace TMS.LMS.MockApi.Controllers;

[ApiController]
[Route("api/lms")]
public class LmsController : ControllerBase
{
    [HttpGet("training-schedule")]
    public IActionResult GetTrainingSchedule()
    {
        return Ok(new[]
        {
            new { SessionCode = "TS-APR-001", DomainName = "Aircraft Training", Module = "A320 Hydraulics", StartOnUtc = DateTime.UtcNow.Date.AddHours(9), EndOnUtc = DateTime.UtcNow.Date.AddHours(16) }
        });
    }

    [HttpGet("modules/{moduleId}")]
    public IActionResult GetModule(string moduleId)
    {
        return Ok(new { ModuleId = moduleId, Name = "A320 Hydraulics", DurationHours = 7.0, IsOvertimeAllowed = false });
    }
}
