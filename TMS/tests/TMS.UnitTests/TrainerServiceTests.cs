using TMS.Application.Contracts;
using TMS.Application.Services;
using TMS.Infrastructure.Repositories;

namespace TMS.UnitTests;

public class TrainerServiceTests
{
    private readonly ITrainerService _trainerService;

    public TrainerServiceTests()
    {
        _trainerService = new TrainerService(new InMemoryTrainerRepository());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnSeededTrainers()
    {
        var result = await _trainerService.GetAllAsync();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_WithUnknownId_ShouldReturnNull()
    {
        var result = await _trainerService.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }
}
