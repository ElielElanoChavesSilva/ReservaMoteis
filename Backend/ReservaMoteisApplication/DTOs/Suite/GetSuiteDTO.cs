namespace BookMotelsApplication.DTOs.Suite
{
    public record GetSuiteDTO(
        long Id,
        string Name,
        string Description,
        decimal PricePerPeriod,
        int MaxOccupancy,
        long MotelId,
        byte[]? ImageUrl
    );
}
