using System;
using System.Collections.Generic;

namespace horse_kurs.Models;

public partial class Client
{
    public int IdClient { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string Phone { get; set; } = null!;

    public string LevelOfTraining { get; set; } = null!;

    public string Passport { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string House { get; set; } = null!;

    public string? Flat { get; set; }

    public decimal Balance { get; set; }

    public DateOnly DateOfRegistration { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();

    public virtual ICollection<Participation> Participations { get; set; } = new List<Participation>();
}
