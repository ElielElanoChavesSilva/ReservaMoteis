namespace BookMotelsDomain.Models
{
    public class BillingReportModel
    {
        public long MotelId { get; set; }
        public string MotelName { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
