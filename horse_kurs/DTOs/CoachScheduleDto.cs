namespace horse_kurs.DTOs
{
    public class CoachScheduleDto
    {
        public int IdLesson { get; set; }
        public DateTime Date { get; set; }
        public string LessonType { get; set; } = null!;
        public string ArenaName { get; set; } = null!;
        public string ClientFullName { get; set; } = null!;
        public string ClientLevel { get; set; } = null!;
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;
        public List<string> HorseNames { get; set; } = new();
    }
}