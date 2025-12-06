namespace ReservaMoteisDomain.Entities
{
    public class ReserveEntity
    {
        public long Id { get; set; }
        public Guid IdUser { get; set; }
        public long IdSuite { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public bool IsReserve { get; set; }
    }
}
