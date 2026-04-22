namespace horse_kurs.DTOs
{
    public class RegisterParticipationDto
    {
        public int IdCompetition { get; set; }
        public int IdClient { get; set; }
        public int IdHorse { get; set; }
        public int? StartNumber { get; set; }
    }
}