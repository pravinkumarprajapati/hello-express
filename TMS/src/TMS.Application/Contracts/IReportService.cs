using TMS.Application.DTOs;

namespace TMS.Application.Contracts;

public interface IReportService
{
    Task<IReadOnlyCollection<UtilizationReportRowDto>> GetUtilizationReportAsync(UtilizationReportFilterDto filter, CancellationToken cancellationToken = default);
    Task<byte[]> ExportUtilizationExcelAsync(UtilizationReportFilterDto filter, CancellationToken cancellationToken = default);
}
