namespace BookMotelsApplication.DTOs.Suite;

public class SuiteDTO
{
    public string Nome { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal PricePerPeriod { get; set; }
    public int MaxOccupancy { get; set; }
    public long MotelId { get; set; }
}
