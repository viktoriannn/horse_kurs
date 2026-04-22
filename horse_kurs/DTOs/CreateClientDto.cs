namespace horse_kurs.DTOs
{
    public class CreateClientDto
    {
        public string Surname { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Patronymic { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; } = null!;
        public string LevelOfTraining { get; set; } = "Новичок";
        public string Passport { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string House { get; set; } = null!;
        public string? Flat { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}