using TMS.Application.DTOs;
using TMS.Application.Services;
using TMS.Infrastructure.Repositories;

namespace TMS.UnitTests;

public class ReportServiceTests
{
    [Fact]
    public async Task GetUtilizationReportAsync_ShouldReturnRows()
    {
        var service = new ReportService(new InMemoryTrainerRepository(), new InMemoryTrainingScheduleRepository());

        var rows = await service.GetUtilizationReportAsync(new UtilizationReportFilterDto());

        Assert.NotNull(rows);
        Assert.NotEmpty(rows);
    }

    [Fact]
    public async Task ExportUtilizationExcelAsync_ShouldReturnBytes()
    {
        var service = new ReportService(new InMemoryTrainerRepository(), new InMemoryTrainingScheduleRepository());

        byte[] content = await service.ExportUtilizationExcelAsync(new UtilizationReportFilterDto());

        Assert.NotNull(content);
        Assert.NotEmpty(content);
    }
}
