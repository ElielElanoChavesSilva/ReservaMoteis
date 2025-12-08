using BookMotelsDomain.DTOs;
using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces.Base;

namespace BookMotelsDomain.Interfaces
{
    public interface IMotelRepository : ICrudRepository<long, MotelEntity>
    {
        Task<IEnumerable<BillingReportDTO>> FindBillingReport(long? motelId, int? year, int? month);
    }
}
