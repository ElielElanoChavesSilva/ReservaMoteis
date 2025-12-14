namespace BookMotelsDomain.Projections
{
    public class GetReserveProjection : GetReserveByUserProjection
    {
        public string UserName { get; set; } = string.Empty;
    }
}
