namespace PolicyTracker.API.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public int PolicyId { get; set; }
        public DateTime ClaimDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Open";
        public DateTime CreatedDate { get; set; }

        public Policy Policy { get; set; } = null!;
    }
}