using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace horse_kurs.Models
{
    [Table("AppUsers")]
    public class AppUser
    {
        [Key]
        [Column("IdUser")]
        public int IdUser { get; set; }

        [Required]
        [Column("Login")]
        [MaxLength(50)]
        public string Login { get; set; } = null!;

        [Required]
        [Column("Password")]
        [MaxLength(100)]
        public string Password { get; set; } = null!;

        [Required]
        [Column("Role")]
        [MaxLength(20)]
        public string Role { get; set; } = "Client";
        public int? IdCoach { get; set; }

        [Column("IdClient")]
        public int? IdClient { get; set; }

        [ForeignKey("IdClient")]
        public virtual Client? Client { get; set; }
        [ForeignKey("IdCoach")]
        public virtual Coach? Coach { get; set; }
    }
}