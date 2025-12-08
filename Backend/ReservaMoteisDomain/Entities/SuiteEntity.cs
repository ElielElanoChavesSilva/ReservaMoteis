using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMotelsDomain.Entities
{
    [Table("Suite")]
    public class SuiteEntity
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public decimal PricePerPeriod { get; set; }
        [Required]
        public int MaxOccupancy { get; set; }
        [Required]
        public long MotelId { get; set; }

        [ForeignKey(nameof(MotelId))]
        public MotelEntity Motel { get; set; }
    }
}
