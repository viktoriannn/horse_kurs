namespace horse_kurs.DTOs
{
    public class BookingDataDto
    {
        public List<CoachInfoDto> Coaches { get; set; } = new();
        public List<ArenaInfoDto> Arenas { get; set; } = new();
        public List<HorseInfoDto> AvailableHorses { get; set; } = new();
    }

    public class CoachInfoDto
    {
        public int IdCoach { get; set; }
        public string FullName { get; set; } = null!;
        public string Specialization { get; set; } = null!;
    }

    public class ArenaInfoDto
    {
        public int IdArena { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
    }

    public class HorseInfoDto
    {
        public int IdHorse { get; set; }
        public string Name { get; set; } = null!;
        public string Breed { get; set; } = null!;
        public string LevelOfTraining { get; set; } = null!;
    }
}