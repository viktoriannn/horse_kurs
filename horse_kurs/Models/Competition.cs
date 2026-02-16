using System;
using System.Collections.Generic;

namespace horse_kurs.Models;

public partial class Competition
{
    public int IdCompetition { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly Date { get; set; }

    public string Type { get; set; } = null!;

    public string Level { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int IdArena { get; set; }

    public virtual Arena IdArenaNavigation { get; set; } = null!;

    public virtual ICollection<Participation> Participations { get; set; } = new List<Participation>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<ScheduleArena> ScheduleArenas { get; set; } = new List<ScheduleArena>();
}
