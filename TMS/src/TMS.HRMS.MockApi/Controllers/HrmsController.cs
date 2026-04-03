using Microsoft.AspNetCore.Mvc;

namespace TMS.HRMS.MockApi.Controllers;

[ApiController]
[Route("api/hrms")]
public class HrmsController : ControllerBase
{
    [HttpGet("trainers")]
    public IActionResult GetTrainers()
    {
        return Ok(new[]
        {
            new { EmployeeCode = "EMP001", Name = "Aarav Sharma", Department = "Aircraft Training" },
            new { EmployeeCode = "EMP002", Name = "Meera Nair", Department = "Safety" }
        });
    }

    [HttpGet("leaves")]
    public IActionResult GetLeaves()
    {
        return Ok(new[]
        {
            new { EmployeeCode = "EMP001", LeaveType = "Emergency", StartOnUtc = DateTime.UtcNow.Date.AddHours(8), EndOnUtc = DateTime.UtcNow.Date.AddHours(18) }
        });
    }

    [HttpGet("schedules")]
    public IActionResult GetSchedules()
    {
        return Ok(new[]
        {
            new { SessionCode = "TS-APR-001", DomainName = "Aircraft Training", StartOnUtc = DateTime.UtcNow.Date.AddHours(9), EndOnUtc = DateTime.UtcNow.Date.AddHours(16) }
        });
    }
}
