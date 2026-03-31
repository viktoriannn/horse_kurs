using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace horse_kurs.Models;

public partial class EquestrianClubContext : DbContext
{
    public EquestrianClubContext()
    {
    }

    public EquestrianClubContext(DbContextOptions<EquestrianClubContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Arena> Arenas { get; set; }
    public virtual DbSet<Users> Users { get; set; }
    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Coach> Coaches { get; set; }

    public virtual DbSet<Competition> Competitions { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Horse> Horses { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<LessonHorse> LessonHorses { get; set; }

    public virtual DbSet<Membership> Memberships { get; set; }

    public virtual DbSet<MembershipLesson> MembershipLessons { get; set; }

    public virtual DbSet<Participation> Participations { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<ScheduleArena> ScheduleArenas { get; set; }

    public virtual DbSet<Stall> Stalls { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=-GARMONY-\\SQLEXPRESS;Database=Equestrian_Club;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Arena>(entity =>
        {
            entity.HasKey(e => e.IdArena).HasName("PK__Arena__D120353F116F169D");

            entity.ToTable("Arena");

            entity.Property(e => e.IdArena).HasColumnName("ID_Arena");
            entity.Property(e => e.Coverage).HasMaxLength(30);
            entity.Property(e => e.Length).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Доступен");
            entity.Property(e => e.Type).HasMaxLength(30);
            entity.Property(e => e.Width).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("PK__Client__B5AE4EC8092BAF53");

            entity.ToTable("Client");

            entity.HasIndex(e => e.Passport, "UQ_Client_Passport").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ_Client_Phone").IsUnique();

            entity.Property(e => e.IdClient).HasColumnName("ID_Client");
            entity.Property(e => e.Balance)
                .HasDefaultValueSql("((0.00))")
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.DateOfBirth).HasColumnName("Date_of_birth");
            entity.Property(e => e.DateOfRegistration)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Date_of_registration");
            entity.Property(e => e.Flat).HasMaxLength(10);
            entity.Property(e => e.House).HasMaxLength(10);
            entity.Property(e => e.LevelOfTraining)
                .HasMaxLength(20)
                .HasDefaultValue("Новичок")
                .HasColumnName("Level_of_training");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Passport).HasMaxLength(20);
            entity.Property(e => e.Patronymic).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Street).HasMaxLength(50);
            entity.Property(e => e.Surname).HasMaxLength(50);
        });

        modelBuilder.Entity<Coach>(entity =>
        {
            entity.HasKey(e => e.IdCoach).HasName("PK__Coach__6D1B22C4B2E492B3");

            entity.ToTable("Coach");

            entity.Property(e => e.IdCoach).HasColumnName("ID_Coach");
            entity.Property(e => e.IdEmployee).HasColumnName("ID_Employee");
            entity.Property(e => e.Qualification).HasMaxLength(100);
            entity.Property(e => e.Specialization).HasMaxLength(100);

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Coaches)
                .HasForeignKey(d => d.IdEmployee)
                .HasConstraintName("FK_Coach_Employee");
        });

        modelBuilder.Entity<Competition>(entity =>
        {
            entity.HasKey(e => e.IdCompetition).HasName("PK__Competit__0E2B371D020CE323");

            entity.ToTable("Competition");

            entity.Property(e => e.IdCompetition).HasColumnName("ID_Competition");
            entity.Property(e => e.IdArena).HasColumnName("ID_Arena");
            entity.Property(e => e.Level).HasMaxLength(30);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Запланировано");
            entity.Property(e => e.Type).HasMaxLength(30);

            entity.HasOne(d => d.IdArenaNavigation).WithMany(p => p.Competitions)
                .HasForeignKey(d => d.IdArena)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Competition_Arena");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.IdEmployee).HasName("PK__Employee__D9EE4F36F60984C0");

            entity.ToTable("Employee");

            entity.Property(e => e.IdEmployee).HasColumnName("ID_Employee");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.DateOfBirth).HasColumnName("Date_of_birth");
            entity.Property(e => e.FlatNumber)
                .HasMaxLength(10)
                .HasColumnName("Flat_number");
            entity.Property(e => e.HouseNumber)
                .HasMaxLength(10)
                .HasColumnName("House_number");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Patronymic).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Post).HasMaxLength(50);
            entity.Property(e => e.Street).HasMaxLength(50);
            entity.Property(e => e.Surname).HasMaxLength(50);
        });

        modelBuilder.Entity<Horse>(entity =>
        {
            entity.HasKey(e => e.IdHorse).HasName("PK__Horse__1F05AB7CD385EE49");

            entity.ToTable("Horse");

            entity.HasIndex(e => e.Passport, "UQ_Horse_Passport").IsUnique();

            entity.Property(e => e.IdHorse).HasColumnName("ID_Horse");
            entity.Property(e => e.Breed).HasMaxLength(50);
            entity.Property(e => e.DateOfBirth).HasColumnName("Date_of_birth");
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.IdStall).HasColumnName("ID_Stall");
            entity.Property(e => e.LevelOfTraining)
                .HasMaxLength(20)
                .HasDefaultValue("Начинающий")
                .HasColumnName("Level_of_training");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Passport).HasMaxLength(50);
            entity.Property(e => e.StateOfHealth)
                .HasMaxLength(20)
                .HasDefaultValue("Здорова")
                .HasColumnName("State_of_health");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("В работе");

            entity.HasOne(d => d.IdStallNavigation).WithMany(p => p.Horses)
                .HasForeignKey(d => d.IdStall)
                .HasConstraintName("FK_Horse_Stall");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.IdLesson).HasName("PK__Lesson__67381F3BE8AF0538");

            entity.ToTable("Lesson");

            entity.Property(e => e.IdLesson).HasColumnName("ID_Lesson");
            entity.Property(e => e.IdArena).HasColumnName("ID_Arena");
            entity.Property(e => e.IdClient).HasColumnName("ID_Client");
            entity.Property(e => e.IdCoach).HasColumnName("ID_Coach");
            entity.Property(e => e.Type).HasMaxLength(30);

            entity.HasOne(d => d.IdArenaNavigation).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.IdArena)
                .HasConstraintName("FK_Lesson_Arena");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lesson_Client");

            entity.HasOne(d => d.IdCoachNavigation).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.IdCoach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lesson_Coach");
        });

        modelBuilder.Entity<LessonHorse>(entity =>
        {
            entity.HasKey(e => e.IdLessonHorse).HasName("PK__Lesson_H__B12E82A70E548177");

            entity.ToTable("Lesson_Horse");

            entity.Property(e => e.IdLessonHorse).HasColumnName("ID_Lesson_Horse");
            entity.Property(e => e.IdHorse).HasColumnName("ID_Horse");
            entity.Property(e => e.IdLesson).HasColumnName("ID_Lesson");

            entity.HasOne(d => d.IdHorseNavigation).WithMany(p => p.LessonHorses)
                .HasForeignKey(d => d.IdHorse)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LessonHorse_Horse");

            entity.HasOne(d => d.IdLessonNavigation).WithMany(p => p.LessonHorses)
                .HasForeignKey(d => d.IdLesson)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LessonHorse_Lesson");
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.HasKey(e => e.IdMembership).HasName("PK__Membersh__49C2410B360EE594");

            entity.ToTable("Membership");

            entity.Property(e => e.IdMembership).HasColumnName("ID_Membership");
            entity.Property(e => e.IdClient).HasColumnName("ID_Client");
            entity.Property(e => e.LessonsTotal).HasColumnName("Lessons_Total");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PurchaseDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Purchase_Date");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Активен");
            entity.Property(e => e.Type).HasMaxLength(30);
            entity.Property(e => e.ValidFrom)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Valid_From");
            entity.Property(e => e.ValidUntil).HasColumnName("Valid_Until");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Memberships)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Membership_Client");
        });

        modelBuilder.Entity<MembershipLesson>(entity =>
        {
            entity.HasKey(e => e.IdMembershipLesson).HasName("PK__Membersh__47838CC859C675E3");

            entity.ToTable("Membership_Lesson");

            entity.Property(e => e.IdMembershipLesson).HasColumnName("ID_Membership_Lesson");
            entity.Property(e => e.IdLesson).HasColumnName("ID_Lesson");
            entity.Property(e => e.IdMembership).HasColumnName("ID_Membership");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdLessonNavigation).WithMany(p => p.MembershipLessons)
                .HasForeignKey(d => d.IdLesson)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MembershipLesson_Lesson");

            entity.HasOne(d => d.IdMembershipNavigation).WithMany(p => p.MembershipLessons)
                .HasForeignKey(d => d.IdMembership)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MembershipLesson_Membership");
        });

        modelBuilder.Entity<Participation>(entity =>
        {
            entity.HasKey(e => e.IdParticipation).HasName("PK__Particip__1A686438E5E30A85");

            entity.ToTable("Participation");

            entity.HasIndex(e => new { e.IdCompetition, e.StartNumber }, "UQ_Participation_StartNumber").IsUnique();

            entity.Property(e => e.IdParticipation).HasColumnName("ID_Participation");
            entity.Property(e => e.IdClient).HasColumnName("ID_Client");
            entity.Property(e => e.IdCompetition).HasColumnName("ID_Competition");
            entity.Property(e => e.IdHorse).HasColumnName("ID_Horse");
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Registration_date");
            entity.Property(e => e.ResultPlace).HasColumnName("Result_place");
            entity.Property(e => e.Score).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.StartNumber).HasColumnName("Start_number");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Participations)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Participation_Client");

            entity.HasOne(d => d.IdCompetitionNavigation).WithMany(p => p.Participations)
                .HasForeignKey(d => d.IdCompetition)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Participation_Competition");

            entity.HasOne(d => d.IdHorseNavigation).WithMany(p => p.Participations)
                .HasForeignKey(d => d.IdHorse)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Participation_Horse");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.IdPayment).HasName("PK__Payment__C2118ADEDA806726");

            entity.ToTable("Payment");

            entity.Property(e => e.IdPayment).HasColumnName("ID_Payment");
            entity.Property(e => e.IdCompetition).HasColumnName("ID_Competition");
            entity.Property(e => e.IdLesson).HasColumnName("ID_Lesson");
            entity.Property(e => e.IdMembership).HasColumnName("ID_Membership");
            entity.Property(e => e.MethodPaid)
                .HasMaxLength(20)
                .HasColumnName("Method_paid");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Payment_date");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Завершено");
            entity.Property(e => e.Summa).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdCompetitionNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.IdCompetition)
                .HasConstraintName("FK_Payment_Competition");

            entity.HasOne(d => d.IdLessonNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.IdLesson)
                .HasConstraintName("FK_Payment_Lesson");

            entity.HasOne(d => d.IdMembershipNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.IdMembership)
                .HasConstraintName("FK_Payment_Membership");
        });

        modelBuilder.Entity<ScheduleArena>(entity =>
        {
            entity.HasKey(e => e.IdSchedule).HasName("PK__Schedule__73616218C0A1A969");

            entity.ToTable("Schedule_Arena");

            entity.Property(e => e.IdSchedule).HasColumnName("ID_Schedule");
            entity.Property(e => e.EndTime).HasColumnName("End_time");
            entity.Property(e => e.IdArena).HasColumnName("ID_Arena");
            entity.Property(e => e.IdCompetition).HasColumnName("ID_Competition");
            entity.Property(e => e.IdLesson).HasColumnName("ID_Lesson");
            entity.Property(e => e.StartTime).HasColumnName("Start_time");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Запланировано");

            entity.HasOne(d => d.IdArenaNavigation).WithMany(p => p.ScheduleArenas)
                .HasForeignKey(d => d.IdArena)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_Arena");

            entity.HasOne(d => d.IdCompetitionNavigation).WithMany(p => p.ScheduleArenas)
                .HasForeignKey(d => d.IdCompetition)
                .HasConstraintName("FK_Schedule_Competition");

            entity.HasOne(d => d.IdLessonNavigation).WithMany(p => p.ScheduleArenas)
                .HasForeignKey(d => d.IdLesson)
                .HasConstraintName("FK_Schedule_Lesson");
        });

        modelBuilder.Entity<Stall>(entity =>
        {
            entity.HasKey(e => e.IdStall).HasName("PK__Stall__922B9F5F83530074");

            entity.ToTable("Stall");

            entity.HasIndex(e => e.Number, "UQ_Stall_Number").IsUnique();

            entity.Property(e => e.IdStall).HasColumnName("ID_Stall");
            entity.Property(e => e.IdEmployee).HasColumnName("ID_Employee");
            entity.Property(e => e.Number).HasMaxLength(10);
            entity.Property(e => e.Size).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Свободен");
            entity.Property(e => e.Type).HasMaxLength(20);

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Stalls)
                .HasForeignKey(d => d.IdEmployee)
                .HasConstraintName("FK_Stall_Employee");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
