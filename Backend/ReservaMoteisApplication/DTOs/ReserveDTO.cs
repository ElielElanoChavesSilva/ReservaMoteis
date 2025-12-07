namespace BookMotelsApplication.DTOs;

public class ReserveDTO
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public long SuiteId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public bool IsReserve { get; set; }
}
