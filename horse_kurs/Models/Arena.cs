using System;
using System.Collections.Generic;

namespace horse_kurs.Models;

public partial class Arena
{
    public int IdArena { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Coverage { get; set; } = null!;

    public decimal Length { get; set; }

    public decimal Width { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Competition> Competitions { get; set; } = new List<Competition>();

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual ICollection<ScheduleArena> ScheduleArenas { get; set; } = new List<ScheduleArena>();
}
