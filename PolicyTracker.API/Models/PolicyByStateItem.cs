namespace PolicyTracker.API.Models
{
    public class PolicyByStateItem
    {
        public string State { get; set; } = string.Empty;
        public int PolicyCount { get; set; }
    }
}
