namespace BookMotelsApplication.DTOs
{
    public class MotelDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<SuiteDTO> Suites { get; set; } = new List<SuiteDTO>();
    }
}
