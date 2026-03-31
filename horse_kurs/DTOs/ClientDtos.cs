namespace horse_kurs.DTOs
{
    public class ClientProfileDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public decimal Balance { get; set; }
        public List<MembershipDto> ActiveMemberships { get; set; } = new();
    }

    public class MembershipDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public int RemainingLessons { get; set; }
        public DateOnly ValidUntil { get; set; }
    }
}