namespace PolicyTracker.API.Models
{
    public class Policy
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string PolicyNumber { get; set; } = string.Empty;
        public string PolicyType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PremiumAmount { get; set; }
        public string Status { get; set; } = "Active";
        public DateTime CreatedDate { get; set; }

        public Customer Customer { get; set; } = null!;
        public List<Claim> Claims { get; set; } = new();
    }
}