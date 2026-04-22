namespace horse_kurs.DTOs
{
    public class AdminUserDto
    {
        public int IdUser { get; set; }
        public string Login { get; set; } = null!;
        public string Role { get; set; } = null!;
        public ClientInfoDto? Client { get; set; }
    }

    public class ClientInfoDto
    {
        public int IdClient { get; set; }
        public string Surname { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string Phone { get; set; } = null!;
        public string Passport { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal Balance { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public string LevelOfTraining { get; set; } = null!;
        public int TotalLessons { get; set; }
        public decimal TotalSpent { get; set; }
    }
}