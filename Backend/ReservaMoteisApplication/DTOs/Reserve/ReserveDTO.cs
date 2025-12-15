namespace BookMotelsApplication.DTOs.Reserve;

public record ReserveDTO(
    long SuiteId,
    decimal TotalPrice,
    DateTime CheckIn,
    DateTime CheckOut
);
