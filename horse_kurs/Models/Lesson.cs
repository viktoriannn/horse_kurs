using System;
using System.Collections.Generic;

namespace horse_kurs.Models;

public partial class Lesson
{
    public int IdLesson { get; set; }

    public DateOnly Date { get; set; }

    public string Type { get; set; } = null!;

    public int IdClient { get; set; }

    public int IdCoach { get; set; }

    public int? IdArena { get; set; }

    public virtual Arena? IdArenaNavigation { get; set; }

    public virtual Client IdClientNavigation { get; set; } = null!;

    public virtual Coach IdCoachNavigation { get; set; } = null!;

    public virtual ICollection<LessonHorse> LessonHorses { get; set; } = new List<LessonHorse>();

    public virtual ICollection<MembershipLesson> MembershipLessons { get; set; } = new List<MembershipLesson>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<ScheduleArena> ScheduleArenas { get; set; } = new List<ScheduleArena>();
}
