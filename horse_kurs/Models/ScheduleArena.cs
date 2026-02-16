using System;
using System.Collections.Generic;

namespace horse_kurs.Models;

public partial class ScheduleArena
{
    public int IdSchedule { get; set; }

    public int IdArena { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string Status { get; set; } = null!;

    public int? IdLesson { get; set; }

    public int? IdCompetition { get; set; }

    public virtual Arena IdArenaNavigation { get; set; } = null!;

    public virtual Competition? IdCompetitionNavigation { get; set; }

    public virtual Lesson? IdLessonNavigation { get; set; }
}
