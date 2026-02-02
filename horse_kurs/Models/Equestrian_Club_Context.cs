using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace horse_kurs.Models;

public partial class Equestrian_Club_Context : DbContext
{
    public Equestrian_Club_Context()
    {
    }

    public Equestrian_Club_Context(DbContextOptions<Equestrian_Club_Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Coach> Coaches { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Horse> Horses { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Membership> Memberships { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Stall> Stalls { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=-GARMONY-\\SQLEXPRESS;Database=Equestrian_Club;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("PK__Client__B5AE4EC8EC8B80CC");

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
            entity.HasKey(e => e.IdCoach).HasName("PK__Coach__6D1B22C4030E56B0");

            entity.ToTable("Coach");

            entity.HasIndex(e => e.Telephone, "UQ_Coach_Telephone").IsUnique();

            entity.Property(e => e.IdCoach).HasColumnName("ID_Coach");
            entity.Property(e => e.DateOfBirth).HasColumnName("Date_of_birth");
            entity.Property(e => e.IdEmployee).HasColumnName("ID_Employee");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Patronymic).HasMaxLength(50);
            entity.Property(e => e.Qualification).HasMaxLength(100);
            entity.Property(e => e.Specialization).HasMaxLength(100);
            entity.Property(e => e.Surname).HasMaxLength(50);
            entity.Property(e => e.Telephone).HasMaxLength(20);

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Coaches)
                .HasForeignKey(d => d.IdEmployee)
                .HasConstraintName("FK_Coach_Employee");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.IdEmployee).HasName("PK__Employee__D9EE4F3695F155E5");

            entity.ToTable("Employee");

            entity.Property(e => e.IdEmployee).HasColumnName("ID_Employee");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.DateOfBirth).HasColumnName("Date_of_birth");
            entity.Property(e => e.FlatNumber)
                .HasMaxLength(10)
                .HasColumnName("Flat_number");
            entity.Property(e => e.HireDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Hire_date");
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
            entity.HasKey(e => e.IdHorse).HasName("PK__Horse__1F05AB7C068B920B");

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
            entity.HasKey(e => e.IdLesson).HasName("PK__Lesson__67381F3B3ECAF7EE");

            entity.ToTable("Lesson");

            entity.Property(e => e.IdLesson).HasColumnName("ID_Lesson");
            entity.Property(e => e.EndTime).HasColumnName("End_time");
            entity.Property(e => e.IdClient).HasColumnName("ID_Client");
            entity.Property(e => e.IdCoach).HasColumnName("ID_Coach");
            entity.Property(e => e.IdHorse).HasColumnName("ID_Horse");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(20)
                .HasColumnName("Payment_type");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StartTime).HasColumnName("Start_time");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Запланировано");
            entity.Property(e => e.Type).HasMaxLength(20);

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lesson_Client");

            entity.HasOne(d => d.IdCoachNavigation).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.IdCoach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lesson_Coach");

            entity.HasOne(d => d.IdHorseNavigation).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.IdHorse)
                .HasConstraintName("FK_Lesson_Horse");
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.HasKey(e => e.IdMembership).HasName("PK__Membersh__49C2410B365DE3F0");

            entity.ToTable("Membership");

            entity.Property(e => e.IdMembership).HasColumnName("ID_Membership");
            entity.Property(e => e.IdClient).HasColumnName("ID_Client");
            entity.Property(e => e.LessonsTotal).HasColumnName("Lessons_Total");
            entity.Property(e => e.LessonsUsed).HasColumnName("Lessons_Used");
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

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.IdPayment).HasName("PK__Payment__C2118ADEA7565AAC");

            entity.ToTable("Payment");

            entity.Property(e => e.IdPayment).HasColumnName("ID_Payment");
            entity.Property(e => e.IdLesson).HasColumnName("ID_Lesson");
            entity.Property(e => e.IdMembership).HasColumnName("ID_Membership");
            entity.Property(e => e.MethodPaid)
                .HasMaxLength(20)
                .HasColumnName("Method_paid");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Payment_date");
            entity.Property(e => e.PurposeOfThePayment)
                .HasMaxLength(50)
                .HasColumnName("Purpose_of_the_payment");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Завершено");
            entity.Property(e => e.Summa).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdLessonNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.IdLesson)
                .HasConstraintName("FK_Payment_Lesson");

            entity.HasOne(d => d.IdMembershipNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.IdMembership)
                .HasConstraintName("FK_Payment_Membership");
        });

        modelBuilder.Entity<Stall>(entity =>
        {
            entity.HasKey(e => e.IdStall).HasName("PK__Stall__922B9F5FBB4B4B3C");

            entity.ToTable("Stall");

            entity.HasIndex(e => e.Number, "UQ_Stall_Number").IsUnique();

            entity.Property(e => e.IdStall).HasColumnName("ID_Stall");
            entity.Property(e => e.IdEmployee).HasColumnName("ID_Employee");
            entity.Property(e => e.LastCleaningDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Last_cleaning_date");
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
