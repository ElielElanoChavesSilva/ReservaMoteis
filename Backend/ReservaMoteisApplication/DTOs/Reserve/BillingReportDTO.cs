namespace BookMotelsApplication.DTOs.Reserve
{
    public class BillingReportDTO
    {
        public long MotelId { get; set; }
        public string MotelName { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
