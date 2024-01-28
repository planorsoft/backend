namespace Planor.Application.Reports.Queries.GetProjectsSummary;

public class GetProjectsSummaryDto
{
    public int CompletedCount { get; set; }
    public int ActiveCount { get; set; }
    public int TotalCount { get; set; }
}