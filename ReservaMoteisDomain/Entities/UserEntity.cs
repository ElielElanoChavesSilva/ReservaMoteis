using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservaMoteisDomain.Entities
{
    [Table("User")]
    public class UserEntity
    {
        [Key] public Guid Id { get; set; }
        [Required][MaxLength(150)] public string Name { get; set; } = string.Empty;
        [Required][MaxLength(200)] public string Email { get; set; } = string.Empty;
        [Required][MaxLength(200)] public string Password { get; set; } = string.Empty;
        [Required] public int ProfileId { get; set; }

        [ForeignKey(nameof(ProfileId))]
        public ProfileEntity Profile { get; set; }
        public ICollection<ReserveEntity> Reserves { get; set; } = new List<ReserveEntity>();
    }
}