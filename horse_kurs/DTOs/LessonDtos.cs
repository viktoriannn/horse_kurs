namespace horse_kurs.DTOs
{
    public class LessonViewDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = null!;
        public string ClientName { get; set; } = null!;
        public string CoachName { get; set; } = null!;
        public string HorseName { get; set; } = null!;
    }

    public class LessonCreateDto
    {
        public DateTime Date { get; set; }
        public string Type { get; set; } = null!;
        public int ClientId { get; set; }
        public int CoachId { get; set; }
        public int HorseId { get; set; }
        public int? ArenaId { get; set; }
    }
}