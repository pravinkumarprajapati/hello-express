using TMS.Application.DTOs;
using TMS.Application.Services;
using TMS.Infrastructure.Repositories;

namespace TMS.UnitTests;

public class NotificationServiceTests
{
    [Fact]
    public async Task SendAsync_WithEnabledConfiguration_ShouldSendEmailAndSms()
    {
        var service = new NotificationService(
            new InMemoryNotificationRepository(),
            new SendGridEmailSender(),
            new AzureCommunicationSmsSender());

        var result = await service.SendAsync(new NotificationRequestDto
        {
            EventKey = "AssignmentChanged",
            RecipientEmail = "trainer@example.com",
            RecipientPhone = "+15550001111",
            TemplateData = "Old: DEL-Virtual, New: BOM-Contact"
        });

        Assert.True(result.EmailAttempted);
        Assert.True(result.SmsAttempted);
        Assert.True(result.EmailSent);
        Assert.True(result.SmsSent);
    }

    [Fact]
    public async Task SaveConfigurationAsync_ShouldDisableSms()
    {
        var service = new NotificationService(
            new InMemoryNotificationRepository(),
            new SendGridEmailSender(),
            new AzureCommunicationSmsSender());

        await service.SaveConfigurationAsync(new NotificationConfigurationDto
        {
            EventKey = "AssignmentChanged",
            EnableEmail = true,
            EnableSms = false
        });

        NotificationConfigurationDto config = await service.GetConfigurationAsync("AssignmentChanged");
        Assert.True(config.EnableEmail);
        Assert.False(config.EnableSms);
    }
}
