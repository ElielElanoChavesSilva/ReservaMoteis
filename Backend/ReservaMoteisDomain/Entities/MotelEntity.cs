using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMotelsDomain.Entities
{
    [Table("Motel")]
    public class MotelEntity
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;
        [Required]
        [MaxLength(250)]
        public string Address { get; set; } = string.Empty;
        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        public ICollection<SuiteEntity>? Suites { get; set; } = new List<SuiteEntity>();
    }
}
