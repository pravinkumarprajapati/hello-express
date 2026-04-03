using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Contracts;
using TMS.Application.DTOs;

namespace TMS.Api.Controllers;

[ApiController]
[Authorize(Policy = "AdminPolicy")]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost("send")]
    [ProducesResponseType(typeof(NotificationDispatchResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Send(NotificationRequestDto request, CancellationToken cancellationToken)
    {
        NotificationDispatchResultDto result = await _notificationService.SendAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("templates/{eventKey}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<NotificationTemplateDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTemplates(string eventKey, CancellationToken cancellationToken)
    {
        var result = await _notificationService.GetTemplatesAsync(eventKey, cancellationToken);
        return Ok(result);
    }

    [HttpPut("templates")]
    public async Task<IActionResult> SaveTemplate(NotificationTemplateDto template, CancellationToken cancellationToken)
    {
        await _notificationService.SaveTemplateAsync(template, cancellationToken);
        return NoContent();
    }

    [HttpGet("configuration/{eventKey}")]
    [ProducesResponseType(typeof(NotificationConfigurationDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetConfiguration(string eventKey, CancellationToken cancellationToken)
    {
        NotificationConfigurationDto config = await _notificationService.GetConfigurationAsync(eventKey, cancellationToken);
        return Ok(config);
    }

    [HttpPut("configuration")]
    public async Task<IActionResult> SaveConfiguration(NotificationConfigurationDto configuration, CancellationToken cancellationToken)
    {
        await _notificationService.SaveConfigurationAsync(configuration, cancellationToken);
        return NoContent();
    }
}
