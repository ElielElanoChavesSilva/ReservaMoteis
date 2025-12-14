namespace BookMotelsDomain.Projections
{
    public record BillingReportProjection(
        long MotelId,
        string MotelName,
        int Year,
        int Month,
        decimal TotalRevenue);
}
