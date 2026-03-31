using System.ComponentModel.DataAnnotations;

namespace horse_kurs.Models
{
    public partial class Users
    {
        [Key]
        public int IdUser { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!; 
        public int? IdClient { get; set; }

        public virtual Client? Client { get; set; }
    }
}
