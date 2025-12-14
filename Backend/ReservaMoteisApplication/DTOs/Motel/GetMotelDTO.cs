using BookMotelsApplication.DTOs.Suite;

namespace BookMotelsApplication.DTOs.Motel
{
    public record GetMotelDTO(
        long Id,
        string Name,
        string Address,
        string Phone,
        string Description,
        ICollection<GetSuiteDTO>? Suites
    );
}
