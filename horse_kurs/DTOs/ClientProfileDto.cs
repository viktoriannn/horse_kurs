namespace horse_kurs.DTOs
{
    public class ClientProfileDto
    {
        public int IdClient { get; set; }
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string LevelOfTraining { get; set; } = null!;
        public decimal Balance { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public int TotalLessons { get; set; }
        public List<MembershipDto> ActiveMemberships { get; set; } = new();
        public List<ClientLessonDto> UpcomingLessons { get; set; } = new();
        public List<CompetitionDto> AvailableCompetitions { get; set; } = new();
        public List<MembershipDto> ActiveMembershipsList { get; set; } = new();
    }

}