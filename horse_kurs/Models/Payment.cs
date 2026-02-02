using System;
using System.Collections.Generic;

namespace horse_kurs.Models;

public partial class Payment
{
    public int IdPayment { get; set; }

    public string MethodPaid { get; set; } = null!;

    public string PurposeOfThePayment { get; set; } = null!;

    public decimal Summa { get; set; }

    public DateTime PaymentDate { get; set; }

    public string Status { get; set; } = null!;

    public int? IdLesson { get; set; }

    public int? IdMembership { get; set; }

    public virtual Lesson? IdLessonNavigation { get; set; }

    public virtual Membership? IdMembershipNavigation { get; set; }
}
