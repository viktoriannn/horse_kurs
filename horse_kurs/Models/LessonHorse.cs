using System;
using System.Collections.Generic;

namespace horse_kurs.Models;

public partial class LessonHorse
{
    public int IdLessonHorse { get; set; }

    public int IdLesson { get; set; }

    public int IdHorse { get; set; }

    public virtual Horse IdHorseNavigation { get; set; } = null!;

    public virtual Lesson IdLessonNavigation { get; set; } = null!;
}
