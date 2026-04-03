using Microsoft.AspNetCore.Mvc.RazorPages;
using TMS.Web.Models;
using TMS.Web.Services;

namespace TMS.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IRosterViewService _rosterViewService;

    public IndexModel(IRosterViewService rosterViewService)
    {
        _rosterViewService = rosterViewService;
    }

    public IReadOnlyCollection<RosterEntryViewModel> RosterEntries { get; private set; } = Array.Empty<RosterEntryViewModel>();

    public async Task OnGetAsync()
    {
        RosterEntries = await _rosterViewService.GetTrainerRosterAsync("Aarav Sharma");
    }
}
