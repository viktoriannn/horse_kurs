using System;
using System.Collections.Generic;

namespace horse_kurs.Models;

public partial class MembershipLesson
{
    public int IdMembershipLesson { get; set; }

    public decimal Price { get; set; }

    public int IdMembership { get; set; }

    public int IdLesson { get; set; }

    public virtual Lesson IdLessonNavigation { get; set; } = null!;

    public virtual Membership IdMembershipNavigation { get; set; } = null!;
}
