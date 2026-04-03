using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Contracts;
using TMS.Application.DTOs;

namespace TMS.Api.Controllers;

[ApiController]
[Authorize(Policy = "TrainerPolicy")]
[Route("api/[controller]")]
public class TrainersController : ControllerBase
{
    private readonly ITrainerService _trainerService;

    public TrainersController(ITrainerService trainerService)
    {
        _trainerService = trainerService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<TrainerProfileDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyCollection<TrainerProfileDto> trainers = await _trainerService.GetAllAsync(cancellationToken);
        return Ok(trainers);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TrainerProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        TrainerProfileDto? trainer = await _trainerService.GetByIdAsync(id, cancellationToken);

        if (trainer is null)
        {
            return NotFound();
        }

        return Ok(trainer);
    }
}
