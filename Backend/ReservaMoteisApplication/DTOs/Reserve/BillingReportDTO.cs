namespace BookMotelsApplication.DTOs.Reserve
{
    public record BillingReportDTO(
        long MotelId,
        string MotelName,
        int Year,
        int Month,
        decimal TotalRevenue
    );
}
