using TMS.Web.Services;

namespace TMS.UnitTests;

public class RosterViewServiceTests
{
    [Fact]
    public async Task GetConsolidatedRosterAsync_ShouldReturnRows()
    {
        var service = new InMemoryRosterViewService();

        var rows = await service.GetConsolidatedRosterAsync();

        Assert.NotNull(rows);
        Assert.NotEmpty(rows);
    }
}
