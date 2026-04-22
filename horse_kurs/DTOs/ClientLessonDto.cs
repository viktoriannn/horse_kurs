namespace horse_kurs.DTOs
{
    public class ClientLessonDto
    {
        public int IdLesson { get; set; }
        public DateTime Date { get; set; }
        public string LessonType { get; set; } = null!;
        public string ArenaName { get; set; } = null!;
        public string CoachName { get; set; } = null!;
        public List<string> HorseNames { get; set; } = new();
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;
    }
}