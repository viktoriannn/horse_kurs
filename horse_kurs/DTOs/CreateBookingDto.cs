namespace horse_kurs.DTOs
{
    public class CreateBookingDto
    {
        public int IdClient { get; set; }
        public int IdCoach { get; set; }
        public int IdArena { get; set; }
        public DateTime Date { get; set; }
        public string LessonType { get; set; } = null!;
        public int? IdHorse { get; set; }
    }
}