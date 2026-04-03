using TMS.Web.Models;

namespace TMS.Web.Services;

public interface IRosterViewService
{
    Task<IReadOnlyCollection<RosterEntryViewModel>> GetTrainerRosterAsync(string trainerName, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<RosterEntryViewModel>> GetConsolidatedRosterAsync(CancellationToken cancellationToken = default);
}
