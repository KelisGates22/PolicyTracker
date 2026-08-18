namespace PolicyTracker.API.Models;

public class OpenClaimReportItem
{
    public string PolicyNumber { get; set; } = string.Empty;
    public string PolicyType { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime ClaimDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
}