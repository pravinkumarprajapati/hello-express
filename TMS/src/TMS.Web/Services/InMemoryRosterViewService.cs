using TMS.Web.Models;

namespace TMS.Web.Services;

/// <summary>
/// Temporary roster provider for Razor Pages UI bootstrap.
/// </summary>
public class InMemoryRosterViewService : IRosterViewService
{
    private static readonly List<RosterEntryViewModel> Entries =
    [
        new()
        {
            TrainerName = "Aarav Sharma",
            Department = "Aircraft Training",
            Date = DateTime.UtcNow.Date.ToString("yyyy-MM-dd"),
            TimeSlot = "09:00 - 16:00",
            Location = "Delhi",
            Mode = "Contact",
            ActivityType = "Training"
        },
        new()
        {
            TrainerName = "Meera Nair",
            Department = "Safety",
            Date = DateTime.UtcNow.Date.AddDays(1).ToString("yyyy-MM-dd"),
            TimeSlot = "10:00 - 14:00",
            Location = "Virtual",
            Mode = "Virtual",
            ActivityType = "Learning Day"
        }
    ];

    public Task<IReadOnlyCollection<RosterEntryViewModel>> GetTrainerRosterAsync(string trainerName, CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<RosterEntryViewModel> roster = Entries
            .Where(x => x.TrainerName.Equals(trainerName, StringComparison.OrdinalIgnoreCase))
            .ToArray();

        return Task.FromResult(roster);
    }

    public Task<IReadOnlyCollection<RosterEntryViewModel>> GetConsolidatedRosterAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult((IReadOnlyCollection<RosterEntryViewModel>)Entries);
    }
}
