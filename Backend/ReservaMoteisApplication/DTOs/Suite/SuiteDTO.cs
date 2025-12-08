namespace BookMotelsApplication.DTOs.Suite;

public class SuiteDTO
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal PricePerPeriod { get; set; }
    public int MaxOccupancy { get; set; }
    public long MotelId { get; set; }
    public bool IsAvailable { get; set; }
}
