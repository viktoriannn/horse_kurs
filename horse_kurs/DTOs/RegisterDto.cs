namespace horse_kurs.DTOs
{
    public class RegisterDto
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int IdClient { get; set; }
    }
}