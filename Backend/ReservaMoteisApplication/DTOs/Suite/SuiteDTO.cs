namespace BookMotelsApplication.DTOs.Suite;

public record SuiteDTO(
    string Name,
    string Description,
    decimal PricePerPeriod,
    int MaxOccupancy,
    byte[]? ImageUrl
);
