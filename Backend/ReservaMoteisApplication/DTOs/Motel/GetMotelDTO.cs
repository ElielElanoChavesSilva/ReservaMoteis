using BookMotelsApplication.DTOs.Suite;

namespace BookMotelsApplication.DTOs.Motel
{
    public class GetMotelDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<GetSuiteDTO>? Suites { get; set; } = new List<GetSuiteDTO>();
    }
}
