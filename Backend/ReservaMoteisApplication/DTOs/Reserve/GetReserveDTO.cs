namespace BookMotelsApplication.DTOs.Reserve
{
    public class GetReserveDTO : GetReserveByUserDTO
    {
        public string UserName { get; set; } = string.Empty;
    }
}
