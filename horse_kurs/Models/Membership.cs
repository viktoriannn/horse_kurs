using System;
using System.Collections.Generic;

namespace horse_kurs.Models;

public partial class Membership
{
    public int IdMembership { get; set; }

    public string Type { get; set; } = null!;

    public int LessonsTotal { get; set; }

    public int LessonsUsed { get; set; }

    public DateOnly ValidFrom { get; set; }

    public DateOnly ValidUntil { get; set; }

    public decimal Price { get; set; }

    public DateOnly PurchaseDate { get; set; }

    public string Status { get; set; } = null!;

    public int IdClient { get; set; }

    public virtual Client IdClientNavigation { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
