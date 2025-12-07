using BookMotelsApplication.DTOs.Suite;

namespace BookMotelsApplication.DTOs.Motel
{
    public class MotelDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<SuiteDTO>? Suites { get; set; } = new List<SuiteDTO>();
    }
}
