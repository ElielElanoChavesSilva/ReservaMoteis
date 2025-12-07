using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMotelsDomain.Entities
{
    [Table("Profile")]
    public class ProfileEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;
    }
}
