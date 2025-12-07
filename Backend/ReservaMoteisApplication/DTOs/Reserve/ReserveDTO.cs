namespace BookMotelsApplication.DTOs.Reserve;

public class ReserveDTO
{
    public Guid UserId { get; set; }
    public long SuiteId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public bool IsReserve { get; set; }
}
