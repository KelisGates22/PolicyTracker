using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyTracker.API.Data;

namespace PolicyTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PoliciesController : ControllerBase
{
    #region Fields and Constructor

    private readonly PolicyTrackerDbContext _db;
    private readonly ILogger<PoliciesController> _logger;

    public PoliciesController(PolicyTrackerDbContext db, ILogger<PoliciesController> logger)
    {
        _db = db;
        _logger = logger;
    }

    #endregion

    #region Endpoints


    /** Gets a list of all policies, including customer information.
     * GET /api/policies
     * 
     * @returns A list of policies with their basic information and associated customer names.
     */
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Fetching all policies");

        var policies = await _db.Policies
            .Include(p => p.Customer)
            .Select(p => new
            {
                p.Id,
                p.PolicyNumber,
                p.PolicyType,
                p.Status,
                p.PremiumAmount,
                p.StartDate,
                p.EndDate,
                CustomerName = p.Customer.FirstName + " " + p.Customer.LastName
            })
            .ToListAsync();

        return Ok(policies);
    }

    /** Gets a specific policy by ID, including customer and claims information.
     * GET /api/policies/{id}
     * 
     * @param id The ID of the policy to retrieve.
     * @returns The policy details along with associated customer and claims information.
     */
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        _logger.LogInformation("Fetching policy {PolicyId}", id);

        var policy = await _db.Policies
            .Include(p => p.Customer)
            .Include(p => p.Claims)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (policy is null)
        {
            _logger.LogWarning("Policy {PolicyId} not found", id);
            return NotFound();
        }

        return Ok(new
        {
            policy.Id,
            policy.PolicyNumber,
            policy.PolicyType,
            policy.Status,
            policy.PremiumAmount,
            policy.StartDate,
            policy.EndDate,
            Customer = new
            {
                policy.Customer.Id,
                policy.Customer.FirstName,
                policy.Customer.LastName
            },
            Claims = policy.Claims.Select(c => new
            {
                c.Id,
                c.ClaimDate,
                c.Description,
                c.Amount,
                c.Status
            })
        });
    }

    #endregion
}