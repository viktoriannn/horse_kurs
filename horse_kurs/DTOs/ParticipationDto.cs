namespace horse_kurs.DTOs
{
    public class ParticipationDto
    {
        public int IdParticipation { get; set; }
        public int IdCompetition { get; set; }
        public string CompetitionName { get; set; } = null!;
        public DateTime CompetitionDate { get; set; }
        public string ClientName { get; set; } = null!;
        public string HorseName { get; set; } = null!;
        public int? StartNumber { get; set; }
        public int? ResultPlace { get; set; }
        public decimal? Score { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}