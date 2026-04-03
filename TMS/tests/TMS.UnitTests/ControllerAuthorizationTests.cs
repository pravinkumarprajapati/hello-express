using Microsoft.AspNetCore.Authorization;
using TMS.Api.Controllers;

namespace TMS.UnitTests;

public class ControllerAuthorizationTests
{
    [Theory]
    [InlineData(typeof(TrainersController), "TrainerPolicy")]
    [InlineData(typeof(AssignmentsController), "ManagerPolicy")]
    [InlineData(typeof(ReportsController), "ManagerPolicy")]
    [InlineData(typeof(SyncController), "ManagerPolicy")]
    [InlineData(typeof(NotificationsController), "AdminPolicy")]
    public void Controllers_ShouldHaveExpectedAuthorizePolicy(Type controllerType, string expectedPolicy)
    {
        var attribute = controllerType.GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true)
            .Cast<AuthorizeAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal(expectedPolicy, attribute!.Policy);
    }
}
