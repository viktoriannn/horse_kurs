using System;
using System.Collections.Generic;

namespace horse_kurs.Models;

public partial class Employee
{
    public int IdEmployee { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string HouseNumber { get; set; } = null!;

    public string? FlatNumber { get; set; }

    public string Post { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ICollection<Coach> Coaches { get; set; } = new List<Coach>();

    public virtual ICollection<Stall> Stalls { get; set; } = new List<Stall>();
}
