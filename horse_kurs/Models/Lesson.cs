using System;
using System.Collections.Generic;

namespace horse_kurs.Models;

public partial class Lesson
{
    public int IdLesson { get; set; }

    public DateOnly Date { get; set; }

    public string Type { get; set; } = null!;

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string Status { get; set; } = null!;

    public decimal Price { get; set; }

    public string? PaymentType { get; set; }

    public int? IdHorse { get; set; }

    public int IdClient { get; set; }

    public int IdCoach { get; set; }

    public virtual Client IdClientNavigation { get; set; } = null!;

    public virtual Coach IdCoachNavigation { get; set; } = null!;

    public virtual Horse? IdHorseNavigation { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
