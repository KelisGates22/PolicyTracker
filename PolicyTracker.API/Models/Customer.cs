namespace PolicyTracker.API.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string SSN { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string State { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }

        public List<Policy> Policies { get; set; } = new();
    }
}
