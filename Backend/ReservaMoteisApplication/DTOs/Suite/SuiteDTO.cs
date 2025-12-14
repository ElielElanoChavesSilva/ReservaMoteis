using Microsoft.AspNetCore.Http;

namespace BookMotelsApplication.DTOs.Suite;

public record SuiteDTO(
    string Name,
    string Description,
    decimal PricePerPeriod,
    int MaxOccupancy,
    IFormFile? Image
);
