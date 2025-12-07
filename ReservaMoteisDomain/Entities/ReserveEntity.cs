using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMotelsDomain.Entities
{
    [Table("Reserve")]
    public class ReserveEntity
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public long SuiteId { get; set; }
        [Required]
        public DateTime CheckIn { get; set; }
        [Required]
        public DateTime CheckOut { get; set; }
        [Required]
        public bool IsReserve { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }
        [ForeignKey(nameof(SuiteId))]
        public SuiteEntity Suite { get; set; }
    }
}