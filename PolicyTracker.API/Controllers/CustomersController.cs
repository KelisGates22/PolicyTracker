using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyTracker.API.Data;

namespace PolicyTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{

    #region Fields and Constructor

    private readonly PolicyTrackerDbContext _db;
    private readonly ILogger<CustomersController> _logger;

    /** Initializes a new instance of the CustomersController class.
     * 
     * @param db The PolicyTrackerDbContext instance.
     * @param logger The ILogger instance for logging.
     * 
     */
    public CustomersController(PolicyTrackerDbContext db, ILogger<CustomersController> logger)
    {
        _db = db;
        _logger = logger;
    }

    #endregion

    #region Endpoints

    /** Gets a list of all customers.
     * GET /api/customers
     * 
     * @returns A list of customers with their basic information.
     */
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Fetching all customers");

        var customers = await _db.Customers
            .Select(c => new
            {
                c.Id,
                c.FirstName,
                c.LastName,
                c.Email,
                c.State
            })
            .ToListAsync();

        return Ok(customers);
    }

    /** Gets a specific customer by ID, including their policies.
     * GET /api/customers/{id}
     * 
     * @param id The ID of the customer to retrieve.
     * @returns The customer details along with their associated policies.
     */
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        _logger.LogInformation("Fetching customer {CustomerId}", id);

        var customer = await _db.Customers
            .Include(c => c.Policies)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (customer is null)
        {
            _logger.LogWarning("Customer {CustomerId} not found", id);
            return NotFound();
        }

        return Ok(new
        {
            customer.Id,
            customer.FirstName,
            customer.LastName,
            SSN = MaskSSN(customer.SSN),
            customer.Email,
            customer.Phone,
            customer.State,
            Policies = customer.Policies.Select(p => new
            {
                p.Id,
                p.PolicyNumber,
                p.PolicyType,
                p.Status,
                p.PremiumAmount
            })
        });
    }

    #endregion

    #region Private Methods

    private static string MaskSSN(string ssn)
    {
        if (string.IsNullOrEmpty(ssn) || ssn.Length < 4)
            return "***-**-****";

        return $"***-**-{ssn[^4..]}";
    }

    #endregion
}