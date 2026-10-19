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

    #region Fields and Constructor

    private readonly PolicyTrackerDbContext _db;
    private readonly ILogger<ReportsController> _logger;
    private readonly IWebHostEnvironment _env;

    public ReportsController(PolicyTrackerDbContext db, ILogger<ReportsController> logger, IWebHostEnvironment env)
    {
        _db = db;
        _logger = logger;
        _env = env;
    }

    #endregion

    #region Endpoints

    /** Generates a report of all open claims and returns it as a PDF file.
     * GET /api/reports/open-claims
     * 
     * @returns A PDF file containing the open claims report.
     */
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

    /** Generates a report chart of policies grouped by state and returns it as a PDF file.
     * GET /api/reports/policies-by-state
     * 
     * @returns A PDF file containing the policies by state report.
     */
    [HttpGet("policies-by-state")]
    public async Task<IActionResult> PoliciesByStateReport()
    {
        _logger.LogInformation("Generating Policies By State Report");

        var data = await _db.Policies
            .GroupBy(p => p.Customer.State)
            .Select(g => new PolicyByStateItem
            {
                State = g.Key,
                PolicyCount = g.Count()
            })
            .OrderBy(x => x.State)
            .ToListAsync();

        var reportPath = Path.Combine(_env.ContentRootPath, "Reports", "PoliciesByStateReport.rdlc");

        using var report = new LocalReport();
        report.ReportPath = reportPath;
        report.DataSources.Add(new ReportDataSource("PolicyByStateData", data));

        var pdfBytes = report.Render("PDF");

        return File(pdfBytes, "application/pdf", "PoliciesByStateReport.pdf");
    }

    #endregion

}