using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyTracker.API.Data;
using PolicyTracker.API.Models;

namespace PolicyTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClaimsController : ControllerBase
{
    #region Fields and Constructor

    private readonly PolicyTrackerDbContext _db;
    private readonly ILogger<ClaimsController> _logger;

    /** Initializes a new instance of the ClaimsController class.
     * 
     * @param db The PolicyTrackerDbContext instance.
     * @param logger The ILogger instance for logging.
     * 
     */
    public ClaimsController(PolicyTrackerDbContext db, ILogger<ClaimsController> logger)
    {
        _db = db;
        _logger = logger;
    }

    #endregion

    #region Endpoints

    /**Gets a list of all claims, including policy information.
     * GET /api/claims
     * 
     * @returns A list of claims with their basic information and associated policy details.
     */
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Fetching all claims");

        var claims = await _db.Claims
            .Include(c => c.Policy)
            .Select(c => new
            {
                c.Id,
                c.ClaimDate,
                c.Description,
                c.Amount,
                c.Status,
                c.Policy.PolicyNumber,
                c.Policy.PolicyType
            })
            .ToListAsync();

        return Ok(claims);
    }

    /** Gets a specific claim by ID, including policy and customer information.
     * GET /api/claims/{id}
     * 
     * @param id The ID of the claim to retrieve.
     * @returns The claim details along with associated policy and customer information.
     */
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        _logger.LogInformation("Fetching claim {ClaimId}", id);

        var claim = await _db.Claims
            .Include(c => c.Policy)
                .ThenInclude(p => p.Customer)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (claim is null)
        {
            _logger.LogWarning("Claim {ClaimId} not found", id);
            return NotFound();
        }

        return Ok(new
        {
            claim.Id,
            claim.ClaimDate,
            claim.Description,
            claim.Amount,
            claim.Status,
            Policy = new
            {
                claim.Policy.PolicyNumber,
                claim.Policy.PolicyType
            },
            Customer = new
            {
                claim.Policy.Customer.FirstName,
                claim.Policy.Customer.LastName
            }
        });
    }

    /** Creates a new claim for a specific policy.
     * POST /api/claims
     * 
     * @param request The request body containing the claim details.
     * @returns The created claim details along with a 201 Created status.
     */
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClaimRequest request)
    {
        _logger.LogInformation("Filing new claim for policy {PolicyId}", request.PolicyId);

        var policy = await _db.Policies.FindAsync(request.PolicyId);

        if (policy is null)
        {
            _logger.LogWarning("Policy {PolicyId} not found", request.PolicyId);
            return NotFound("Policy not found");
        }

        var claim = new Claim
        {
            Id = Guid.NewGuid(),
            PolicyId = request.PolicyId,
            ClaimDate = request.ClaimDate,
            Description = request.Description,
            Amount = request.Amount,
            Status = "Open",
            CreatedDate = DateTime.UtcNow
        };

        _db.Claims.Add(claim);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Claim {ClaimId} created successfully", claim.Id);

        return CreatedAtAction(nameof(GetById), new { id = claim.Id }, new
        {
            claim.Id,
            claim.ClaimDate,
            claim.Description,
            claim.Amount,
            claim.Status
        });
    }

    #endregion
}

#region Request Models

/** Represents the request body for creating a new claim.
 * 
 * @property PolicyId The ID of the policy for which the claim is being filed.
 * @property ClaimDate The date of the claim.
 * @property Description A description of the claim.
 * @property Amount The amount being claimed.
 */
public class CreateClaimRequest
{
    public Guid PolicyId { get; set; }
    public DateTime ClaimDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

#endregion