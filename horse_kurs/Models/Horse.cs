using System;
using System.Collections.Generic;

namespace horse_kurs.Models;

public partial class Horse
{
    public int IdHorse { get; set; }

    public string Name { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string Breed { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string StateOfHealth { get; set; } = null!;

    public string LevelOfTraining { get; set; } = null!;

    public string Passport { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int? IdStall { get; set; }

    public virtual Stall? IdStallNavigation { get; set; }

    public virtual ICollection<LessonHorse> LessonHorses { get; set; } = new List<LessonHorse>();

    public virtual ICollection<Participation> Participations { get; set; } = new List<Participation>();
}
