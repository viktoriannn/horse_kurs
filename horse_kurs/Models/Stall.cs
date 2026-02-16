using System;
using System.Collections.Generic;

namespace horse_kurs.Models;

public partial class Stall
{
    public int IdStall { get; set; }

    public string Number { get; set; } = null!;

    public string Type { get; set; } = null!;

    public decimal Size { get; set; }

    public string Status { get; set; } = null!;

    public int? IdEmployee { get; set; }

    public virtual ICollection<Horse> Horses { get; set; } = new List<Horse>();

    public virtual Employee? IdEmployeeNavigation { get; set; }
}
