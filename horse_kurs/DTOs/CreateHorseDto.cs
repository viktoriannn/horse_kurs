namespace horse_kurs.DTOs
{
    public class CreateHorseDto
    {
        public string Name { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Breed { get; set; } = null!;
        public DateTime DateOfBirth { get; set; } 
        public string StateOfHealth { get; set; } = "Здорова";
        public string LevelOfTraining { get; set; } = "Начинающий";
        public string Passport { get; set; } = null!;
        public string Status { get; set; } = "В работе";
        public int? IdStall { get; set; }
    }
}