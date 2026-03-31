namespace horse_kurs.DTOs
{
    public class HorseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Breed { get; set; } = null!;
        public string HealthStatus { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
