using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
using PolicyTracker.API.Data;
using PolicyTracker.API.Models;

namespace PolicyTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly PolicyTrackerDbContext _db;
    private readonly ILogger<ReportsController> _logger;
    private readonly IWebHostEnvironment _env;

    public ReportsController(PolicyTrackerDbContext db, ILogger<ReportsController> logger, IWebHostEnvironment env)
    {
        _db = db;
        _logger = logger;
        _env = env;
    }

    // GET /api/reports/open-claims
    [HttpGet("open-claims")]
    public async Task<IActionResult> OpenClaimsReport()
    {
        _logger.LogInformation("Generating Open Claims Report");

        var claims = await _db.Claims
            .Include(c => c.Policy)
                .ThenInclude(p => p.Customer)
            .Where(c => c.Status != "Closed")
            .Select(c => new OpenClaimReportItem
            {
                PolicyNumber = c.Policy.PolicyNumber,
                PolicyType = c.Policy.PolicyType,
                CustomerName = c.Policy.Customer.FirstName + " " + c.Policy.Customer.LastName,
                ClaimDate = c.ClaimDate,
                Description = c.Description,
                Amount = c.Amount,
                Status = c.Status
            })
            .ToListAsync();

        var reportPath = Path.Combine(_env.ContentRootPath, "Reports", "OpenClaimsReport.rdlc");

        using var report = new LocalReport();
        report.ReportPath = reportPath;
        report.DataSources.Add(new ReportDataSource("OpenClaimsData", claims));

        var pdfBytes = report.Render("PDF");

        return File(pdfBytes, "application/pdf", "OpenClaimsReport.pdf");
    }
}