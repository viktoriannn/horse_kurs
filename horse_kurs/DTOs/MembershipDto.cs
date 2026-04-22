namespace horse_kurs.DTOs
{
    public class MembershipDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public int RemainingLessons { get; set; }
        public DateOnly ValidUntil { get; set; }
        public string Status { get; set; } = null!;
    }
}