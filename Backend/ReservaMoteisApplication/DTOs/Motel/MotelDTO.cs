namespace BookMotelsApplication.DTOs.Motel
{
    public record MotelDTO(
        string Name,
        string Address,
        string Phone,
        string Description
    );
}
