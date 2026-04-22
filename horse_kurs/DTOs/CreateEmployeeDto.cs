namespace horse_kurs.DTOs
{
    public class CreateEmployeeDto
    {
        public string Surname { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Patronymic { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string HouseNumber { get; set; } = null!;
        public string? FlatNumber { get; set; }
        public string Post { get; set; } = null!;
        public string? Phone { get; set; }
    }
}