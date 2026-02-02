using System;
using System.Collections.Generic;

namespace horse_kurs.Models;

public partial class Coach
{
    public int IdCoach { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string Telephone { get; set; } = null!;

    public string Qualification { get; set; } = null!;

    public string Specialization { get; set; } = null!;

    public int? IdEmployee { get; set; }

    public virtual Employee? IdEmployeeNavigation { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
