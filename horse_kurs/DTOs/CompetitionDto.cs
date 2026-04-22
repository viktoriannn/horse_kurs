namespace horse_kurs.DTOs
{
    public class CompetitionDto
    {
        public int IdCompetition { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Type { get; set; } = null!;
        public string Level { get; set; } = null!;
        public string Status { get; set; } = null!;
        public decimal RegistrationFee { get; set; }
    }
}