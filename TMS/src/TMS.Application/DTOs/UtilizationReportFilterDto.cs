namespace TMS.Application.DTOs;

public class UtilizationReportFilterDto
{
    public string? Name { get; set; }
    public string? Department { get; set; }
    public DateTime? FromOnUtc { get; set; }
    public DateTime? ToOnUtc { get; set; }
}
