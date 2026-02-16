using System;
using System.Collections.Generic;

namespace horse_kurs.Models;

public partial class Participation
{
    public int IdParticipation { get; set; }

    public DateOnly RegistrationDate { get; set; }

    public int? StartNumber { get; set; }

    public int? ResultPlace { get; set; }

    public decimal? Score { get; set; }

    public int IdCompetition { get; set; }

    public int IdClient { get; set; }

    public int IdHorse { get; set; }

    public virtual Client IdClientNavigation { get; set; } = null!;

    public virtual Competition IdCompetitionNavigation { get; set; } = null!;

    public virtual Horse IdHorseNavigation { get; set; } = null!;
}
