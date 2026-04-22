namespace horse_kurs.DTOs
{
    public class AuthResponseDto
    {
        public int IdUser { get; set; }
        public string Login { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int? IdClient { get; set; }
        public int? IdCoach { get; set; }
        public string? FullName { get; set; }
        public string? Token { get; set; }
    }
}