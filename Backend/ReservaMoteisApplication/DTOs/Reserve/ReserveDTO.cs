namespace BookMotelsApplication.DTOs.Reserve;

public record ReserveDTO(
    long SuiteId,
    DateTime CheckIn,
    DateTime CheckOut
);
