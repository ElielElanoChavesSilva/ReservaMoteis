namespace BookMotelsApplication.DTOs.Reserve
{
    public class GetReserveByUserDTO
    {
        public long Id { get; set; }
        public string SuiteName { get; set; } = string.Empty;
        public string MotelName { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public long SuiteId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
    }
}
