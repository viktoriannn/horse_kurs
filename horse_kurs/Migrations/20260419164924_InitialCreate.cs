using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace horse_kurs.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Arena",
                columns: table => new
                {
                    ID_Arena = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Coverage = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Length = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Width = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Доступен")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Arena__D120353F116F169D", x => x.ID_Arena);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ID_Client = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Patronymic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Level_of_training = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Новичок"),
                    Passport = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    House = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Flat = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValueSql: "((0.00))"),
                    Date_of_registration = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Client__B5AE4EC8092BAF53", x => x.ID_Client);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    ID_Employee = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Patronymic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    House_number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Flat_number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Post = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Employee__D9EE4F36F60984C0", x => x.ID_Employee);
                });

            migrationBuilder.CreateTable(
                name: "Competition",
                columns: table => new
                {
                    ID_Competition = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Level = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Запланировано"),
                    ID_Arena = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Competit__0E2B371D020CE323", x => x.ID_Competition);
                    table.ForeignKey(
                        name: "FK_Competition_Arena",
                        column: x => x.ID_Arena,
                        principalTable: "Arena",
                        principalColumn: "ID_Arena");
                });

            migrationBuilder.CreateTable(
                name: "Membership",
                columns: table => new
                {
                    ID_Membership = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Lessons_Total = table.Column<int>(type: "int", nullable: false),
                    Valid_From = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    Valid_Until = table.Column<DateOnly>(type: "date", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Purchase_Date = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Активен"),
                    ID_Client = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Membersh__49C2410B360EE594", x => x.ID_Membership);
                    table.ForeignKey(
                        name: "FK_Membership_Client",
                        column: x => x.ID_Client,
                        principalTable: "Client",
                        principalColumn: "ID_Client");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdClient = table.Column<int>(type: "int", nullable: true),
                    ClientIdClient = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IdUser);
                    table.ForeignKey(
                        name: "FK_Users_Client_ClientIdClient",
                        column: x => x.ClientIdClient,
                        principalTable: "Client",
                        principalColumn: "ID_Client");
                });

            migrationBuilder.CreateTable(
                name: "Coach",
                columns: table => new
                {
                    ID_Coach = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Qualification = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ID_Employee = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Coach__6D1B22C4B2E492B3", x => x.ID_Coach);
                    table.ForeignKey(
                        name: "FK_Coach_Employee",
                        column: x => x.ID_Employee,
                        principalTable: "Employee",
                        principalColumn: "ID_Employee");
                });

            migrationBuilder.CreateTable(
                name: "Stall",
                columns: table => new
                {
                    ID_Stall = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Size = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Свободен"),
                    ID_Employee = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Stall__922B9F5F83530074", x => x.ID_Stall);
                    table.ForeignKey(
                        name: "FK_Stall_Employee",
                        column: x => x.ID_Employee,
                        principalTable: "Employee",
                        principalColumn: "ID_Employee");
                });

            migrationBuilder.CreateTable(
                name: "Lesson",
                columns: table => new
                {
                    ID_Lesson = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ID_Client = table.Column<int>(type: "int", nullable: false),
                    ID_Coach = table.Column<int>(type: "int", nullable: false),
                    ID_Arena = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Lesson__67381F3BE8AF0538", x => x.ID_Lesson);
                    table.ForeignKey(
                        name: "FK_Lesson_Arena",
                        column: x => x.ID_Arena,
                        principalTable: "Arena",
                        principalColumn: "ID_Arena");
                    table.ForeignKey(
                        name: "FK_Lesson_Client",
                        column: x => x.ID_Client,
                        principalTable: "Client",
                        principalColumn: "ID_Client");
                    table.ForeignKey(
                        name: "FK_Lesson_Coach",
                        column: x => x.ID_Coach,
                        principalTable: "Coach",
                        principalColumn: "ID_Coach");
                });

            migrationBuilder.CreateTable(
                name: "Horse",
                columns: table => new
                {
                    ID_Horse = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Breed = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    State_of_health = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Здорова"),
                    Level_of_training = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Начинающий"),
                    Passport = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "В работе"),
                    ID_Stall = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Horse__1F05AB7CD385EE49", x => x.ID_Horse);
                    table.ForeignKey(
                        name: "FK_Horse_Stall",
                        column: x => x.ID_Stall,
                        principalTable: "Stall",
                        principalColumn: "ID_Stall");
                });

            migrationBuilder.CreateTable(
                name: "Membership_Lesson",
                columns: table => new
                {
                    ID_Membership_Lesson = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ID_Membership = table.Column<int>(type: "int", nullable: false),
                    ID_Lesson = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Membersh__47838CC859C675E3", x => x.ID_Membership_Lesson);
                    table.ForeignKey(
                        name: "FK_MembershipLesson_Lesson",
                        column: x => x.ID_Lesson,
                        principalTable: "Lesson",
                        principalColumn: "ID_Lesson");
                    table.ForeignKey(
                        name: "FK_MembershipLesson_Membership",
                        column: x => x.ID_Membership,
                        principalTable: "Membership",
                        principalColumn: "ID_Membership");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    ID_Payment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Method_paid = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Summa = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Payment_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Завершено"),
                    ID_Lesson = table.Column<int>(type: "int", nullable: true),
                    ID_Membership = table.Column<int>(type: "int", nullable: true),
                    ID_Competition = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payment__C2118ADEDA806726", x => x.ID_Payment);
                    table.ForeignKey(
                        name: "FK_Payment_Competition",
                        column: x => x.ID_Competition,
                        principalTable: "Competition",
                        principalColumn: "ID_Competition");
                    table.ForeignKey(
                        name: "FK_Payment_Lesson",
                        column: x => x.ID_Lesson,
                        principalTable: "Lesson",
                        principalColumn: "ID_Lesson");
                    table.ForeignKey(
                        name: "FK_Payment_Membership",
                        column: x => x.ID_Membership,
                        principalTable: "Membership",
                        principalColumn: "ID_Membership");
                });

            migrationBuilder.CreateTable(
                name: "Schedule_Arena",
                columns: table => new
                {
                    ID_Schedule = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Arena = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Start_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    End_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Запланировано"),
                    ID_Lesson = table.Column<int>(type: "int", nullable: true),
                    ID_Competition = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Schedule__73616218C0A1A969", x => x.ID_Schedule);
                    table.ForeignKey(
                        name: "FK_Schedule_Arena",
                        column: x => x.ID_Arena,
                        principalTable: "Arena",
                        principalColumn: "ID_Arena");
                    table.ForeignKey(
                        name: "FK_Schedule_Competition",
                        column: x => x.ID_Competition,
                        principalTable: "Competition",
                        principalColumn: "ID_Competition");
                    table.ForeignKey(
                        name: "FK_Schedule_Lesson",
                        column: x => x.ID_Lesson,
                        principalTable: "Lesson",
                        principalColumn: "ID_Lesson");
                });

            migrationBuilder.CreateTable(
                name: "Lesson_Horse",
                columns: table => new
                {
                    ID_Lesson_Horse = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Lesson = table.Column<int>(type: "int", nullable: false),
                    ID_Horse = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Lesson_H__B12E82A70E548177", x => x.ID_Lesson_Horse);
                    table.ForeignKey(
                        name: "FK_LessonHorse_Horse",
                        column: x => x.ID_Horse,
                        principalTable: "Horse",
                        principalColumn: "ID_Horse");
                    table.ForeignKey(
                        name: "FK_LessonHorse_Lesson",
                        column: x => x.ID_Lesson,
                        principalTable: "Lesson",
                        principalColumn: "ID_Lesson");
                });

            migrationBuilder.CreateTable(
                name: "Participation",
                columns: table => new
                {
                    ID_Participation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Registration_date = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    Start_number = table.Column<int>(type: "int", nullable: true),
                    Result_place = table.Column<int>(type: "int", nullable: true),
                    Score = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ID_Competition = table.Column<int>(type: "int", nullable: false),
                    ID_Client = table.Column<int>(type: "int", nullable: false),
                    ID_Horse = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Particip__1A686438E5E30A85", x => x.ID_Participation);
                    table.ForeignKey(
                        name: "FK_Participation_Client",
                        column: x => x.ID_Client,
                        principalTable: "Client",
                        principalColumn: "ID_Client");
                    table.ForeignKey(
                        name: "FK_Participation_Competition",
                        column: x => x.ID_Competition,
                        principalTable: "Competition",
                        principalColumn: "ID_Competition");
                    table.ForeignKey(
                        name: "FK_Participation_Horse",
                        column: x => x.ID_Horse,
                        principalTable: "Horse",
                        principalColumn: "ID_Horse");
                });

            migrationBuilder.CreateIndex(
                name: "UQ_Client_Passport",
                table: "Client",
                column: "Passport",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Client_Phone",
                table: "Client",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coach_ID_Employee",
                table: "Coach",
                column: "ID_Employee");

            migrationBuilder.CreateIndex(
                name: "IX_Competition_ID_Arena",
                table: "Competition",
                column: "ID_Arena");

            migrationBuilder.CreateIndex(
                name: "IX_Horse_ID_Stall",
                table: "Horse",
                column: "ID_Stall");

            migrationBuilder.CreateIndex(
                name: "UQ_Horse_Passport",
                table: "Horse",
                column: "Passport",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_ID_Arena",
                table: "Lesson",
                column: "ID_Arena");

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_ID_Client",
                table: "Lesson",
                column: "ID_Client");

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_ID_Coach",
                table: "Lesson",
                column: "ID_Coach");

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_Horse_ID_Horse",
                table: "Lesson_Horse",
                column: "ID_Horse");

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_Horse_ID_Lesson",
                table: "Lesson_Horse",
                column: "ID_Lesson");

            migrationBuilder.CreateIndex(
                name: "IX_Membership_ID_Client",
                table: "Membership",
                column: "ID_Client");

            migrationBuilder.CreateIndex(
                name: "IX_Membership_Lesson_ID_Lesson",
                table: "Membership_Lesson",
                column: "ID_Lesson");

            migrationBuilder.CreateIndex(
                name: "IX_Membership_Lesson_ID_Membership",
                table: "Membership_Lesson",
                column: "ID_Membership");

            migrationBuilder.CreateIndex(
                name: "IX_Participation_ID_Client",
                table: "Participation",
                column: "ID_Client");

            migrationBuilder.CreateIndex(
                name: "IX_Participation_ID_Horse",
                table: "Participation",
                column: "ID_Horse");

            migrationBuilder.CreateIndex(
                name: "UQ_Participation_StartNumber",
                table: "Participation",
                columns: new[] { "ID_Competition", "Start_number" },
                unique: true,
                filter: "[Start_number] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ID_Competition",
                table: "Payment",
                column: "ID_Competition");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ID_Lesson",
                table: "Payment",
                column: "ID_Lesson");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ID_Membership",
                table: "Payment",
                column: "ID_Membership");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Arena_ID_Arena",
                table: "Schedule_Arena",
                column: "ID_Arena");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Arena_ID_Competition",
                table: "Schedule_Arena",
                column: "ID_Competition");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Arena_ID_Lesson",
                table: "Schedule_Arena",
                column: "ID_Lesson");

            migrationBuilder.CreateIndex(
                name: "IX_Stall_ID_Employee",
                table: "Stall",
                column: "ID_Employee");

            migrationBuilder.CreateIndex(
                name: "UQ_Stall_Number",
                table: "Stall",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClientIdClient",
                table: "Users",
                column: "ClientIdClient");

            migrationBuilder.Sql(@"
INSERT INTO Employee (Surname, [Name], Patronymic, Date_of_birth, City, Street, House_number, Flat_number, Post, Phone)
VALUES 
('Иванов', 'Иван', 'Иванович', '19850315', 'Москва', 'Ленина', '10', '25', 'Тренер', '+7 (495) 123-45-67'),
('Петрова', 'Елена', 'Сергеевна', '19900722', 'Москва', 'Пушкина', '5', '12', 'Тренер', '+7 (495) 234-56-78'),
('Сидоров', 'Алексей', 'Петрович', '19781105', 'Москва', 'Гагарина', '15', NULL, 'Ветеринар', '+7 (495) 345-67-89'),
('Козлова', 'Наталья', 'Ивановна', '19880918', 'Москва', 'Мира', '7', '34', 'Администратор', '+7 (495) 456-78-90'),
('Морозов', 'Дмитрий', 'Андреевич', '19820430', 'Москва', 'Садовая', '22', '5', 'Конюх', '+7 (495) 567-89-01'),
('Волкова', 'Анна', 'Викторовна', '19921212', 'Москва', 'Парковая', '3', '18', 'Конюх', '+7 (495) 678-90-12'),
('Соколов', 'Павел', 'Дмитриевич', '19750625', 'Москва', 'Лесная', '8', NULL, 'Менеджер', '+7 (495) 789-01-23'),
('Михайлова', 'Ольга', 'Алексеевна', '19871008', 'Москва', 'Цветочная', '12', '7', 'Уборщик', '+7 (495) 890-12-34'),
('Николаев', 'Сергей', 'Владимирович', '19830214', 'Москва', 'Солнечная', '9', '42', 'Тренер', '+7 (495) 901-23-45'),
('Федорова', 'Татьяна', 'Николаевна', '19910820', 'Москва', 'Березовая', '4', '15', 'Конюх', '+7 (495) 012-34-56');

-- 2. Заполнение таблицы Client (Клиенты)
INSERT INTO Client (Surname, [Name], Patronymic, Date_of_birth, Phone, Level_of_training, Passport, City, Street, House, Flat, Balance, Date_of_registration)
VALUES
('Смирнов', 'Александр', 'Игоревич', '19950510', '+7 (903) 111-22-33', 'Новичок', '4512 123456', 'Москва', 'Тверская', '15', '8', 5000.00, '20250115'),
('Кузнецова', 'Мария', 'Дмитриевна', '19880923', '+7 (903) 222-33-44', 'Любитель', '4512 234567', 'Москва', 'Арбат', '22', '12', 8500.00, '20251120'),
('Попов', 'Денис', 'Алексеевич', '19921201', '+7 (903) 333-44-55', 'Спортсмен', '4512 345678', 'Москва', 'Новый Арбат', '7', '3', 12000.00, '20250910'),
('Васильева', 'Екатерина', 'Сергеевна', '19970317', '+7 (903) 444-55-66', 'Новичок', '4512 456789', 'Москва', 'Красная', '5', '22', 3000.00, '20260205'),
('Михайлов', 'Андрей', 'Петрович', '19850729', '+7 (903) 555-66-77', 'Профессионал', '4512 567890', 'Москва', 'Ленинский', '45', '18', 25000.00, '20250615'),
('Новикова', 'Анна', 'Владимировна', '19941111', '+7 (903) 666-77-88', 'Любитель', '4512 678901', 'Москва', 'Вернадского', '33', '5', 7200.00, '20251201'),
('Зайцев', 'Илья', 'Андреевич', '19900404', '+7 (903) 777-88-99', 'Спортсмен', '4512 789012', 'Москва', 'Мичурина', '12', '9', 15000.00, '20251010'),
('Морозова', 'Ольга', 'Ивановна', '19870819', '+7 (903) 888-99-00', 'Профессионал', '4512 890123', 'Москва', 'Строителей', '8', '16', 22000.00, '20250520'),
('Волков', 'Максим', 'Сергеевич', '19930625', '+7 (903) 999-00-11', 'Новичок', '4512 901234', 'Москва', 'Молодежная', '17', '4', 1500.00, '20260210'),
('Соколова', 'Дарья', 'Алексеевна', '19960130', '+7 (903) 000-11-22', 'Любитель', '4512 012345', 'Москва', 'Победы', '25', '7', 6800.00, '20251215');

-- 3. Заполнение таблицы Stall (Денники)
INSERT INTO Stall ([Number], [Type], Size, [Status], ID_Employee)
VALUES
('A1', 'Стандартный', 14.5, 'Занят', 5),
('A2', 'Стандартный', 15.0, 'Занят', 5),
('A3', 'Стандартный', 14.0, 'Занят', 6),
('A4', 'Стандартный', 15.5, 'Занят', 6),
('B1', 'Большой', 18.0, 'Занят', 5),
('B2', 'Большой', 19.0, 'Занят', 6),
('C1', 'Для пони', 10.5, 'Свободен', NULL),
('C2', 'Для пони', 11.0, 'Свободен', NULL),
('D1', 'Изолятор', 16.0, 'Свободен', 5),
('E1', 'Стандартный', 14.0, 'На уборке', 5),
('E2', 'Стандартный', 14.5, 'Свободен', NULL),
('F1', 'Большой', 18.5, 'Занят', 6);

-- 4. Заполнение таблицы Horse (Лошади)
INSERT INTO Horse ([Name], Gender, Breed, Date_of_birth, State_of_health, Level_of_training, Passport, [Status], ID_Stall)
VALUES
('Гром', 'Жеребец', 'Тракененская', '20180415', 'Здорова', 'Спортивный', 'H12345', 'В работе', 1),
('Заря', 'Кобыла', 'Ганноверская', '20190620', 'Здорова', 'Продвинутый', 'H12346', 'В работе', 2),
('Ветер', 'Мерин', 'Буденновская', '20170310', 'Здорова', 'Профессиональный', 'H12347', 'В работе', 3),
('Роза', 'Кобыла', 'Арабская', '20200805', 'Здорова', 'Начинающий', 'H12348', 'В работе', 4),
('Алмаз', 'Жеребец', 'Ахалтекинская', '20161112', 'Здорова', 'Спортивный', 'H12349', 'В работе', 5),
('Сивка', 'Кобыла', 'Русская верховая', '20180918', 'Здорова', 'Продвинутый', 'H12350', 'В работе', 6),
('Буран', 'Мерин', 'Владимирский тяжеловоз', '20151203', 'Восстанавливается', 'Профессиональный', 'H12351', 'На отдыхе', 9),
('Ласка', 'Кобыла', 'Пони', '20210522', 'Здорова', 'Начинающий', 'H12352', 'В работе', 7),
('Орлик', 'Жеребец', 'Орловский рысак', '20170730', 'Здорова', 'Спортивный', 'H12353', 'В работе', 8),
('Ночка', 'Кобыла', 'Кабардинская', '20191014', 'Здорова', 'Продвинутый', 'H12354', 'В работе', 12);

-- 5. Заполнение таблицы Coach (Тренеры)
INSERT INTO Coach (Qualification, Specialization, ID_Employee)
VALUES
('Мастер спорта по конкуру', 'Конкур', 1),
('КМС по выездке', 'Выездка', 2),
('Тренер высшей категории', 'Детская группа', 9),
('Мастер спорта по вольтижировке', 'Вольтижировка', 4),
('Инструктор по верховой езде', 'Прогулки', 5);

-- 6. Заполнение таблицы Arena (Манежи)
INSERT INTO Arena ([Name], [Type], Coverage, [Length], Width, [Status])
VALUES
('Большой конкурный', 'Конкурный', 'Песок', 60.0, 40.0, 'Доступен'),
('Малый конкурный', 'Конкурный', 'Песок', 40.0, 20.0, 'Доступен'),
('Выездковый', 'Выездковый', 'Резиновая крошка', 50.0, 30.0, 'Доступен'),
('Универсальный', 'Универсальный', 'Смешанный', 45.0, 25.0, 'Доступен'),
('Крытый манеж', 'Крытый', 'Резиновая крошка', 40.0, 20.0, 'Доступен'),
('Прогулочный', 'Прогулочный', 'Трава', 80.0, 50.0, 'Доступен'),
('Летний', 'Открытый', 'Грунт', 50.0, 30.0, 'На обслуживании');

-- 7. Заполнение таблицы Membership (Абонементы)
INSERT INTO Membership ([Type], Lessons_Total, Valid_From, Valid_Until, Price, Purchase_Date, [Status], ID_Client)
VALUES
('Новичок', 4, '20260201', '20260401', 4000.00, '20260201', 'Активен', 1),
('Любитель', 8, '20260115', '20260415', 7200.00, '20260115', 'Активен', 2),
('Спортивный', 12, '20251201', '20260301', 10200.00, '20251201', 'Активен', 3),
('Новичок', 4, '20260210', '20260410', 4000.00, '20260210', 'Активен', 4),
('Профессиональный', 16, '20251101', '20260201', 14400.00, '20251101', 'Использован', 5),
('Любитель', 8, '20260120', '20260420', 7200.00, '20260120', 'Активен', 6),
('Спортивный', 12, '20251015', '20260115', 10200.00, '20251015', 'Просрочен', 7),
('Профессиональный', 16, '20250901', '20251201', 14400.00, '20250901', 'Использован', 8),
('Новичок', 4, '20260215', '20260415', 4000.00, '20260215', 'Активен', 9),
('Любитель', 8, '20251210', '20260310', 7200.00, '20251210', 'Активен', 10);

-- 8. Заполнение таблицы Competition (Соревнования) - с датами после текущей
INSERT INTO Competition ([Name], [Date], [Type], [Level], [Status], ID_Arena)
VALUES
('Кубок Москвы по конкуру', '20260415', 'Конкур', 'Региональные', 'Запланировано', 1),
('Весенний турнир по выездке', '20260520', 'Выездка', 'Любительские', 'Запланировано', 3),
('Детские старты', '20260310', 'Конкур', 'Клубные', 'Запланировано', 2),
('Открытый чемпионат клуба', '20260605', 'Троеборье', 'Клубные', 'Регистрация', 4),
('Кубок России по вольтижировке', '20260712', 'Вольтижировка', 'Национальные', 'Запланировано', 5),
('Зимний кубок', '20260220', 'Конкур', 'Любительские', 'Завершено', 1),
('Новогодний турнир', '20251225', 'Выездка', 'Клубные', 'Завершено', 3);

-- 9. Заполнение таблицы Lesson (Занятия) - с датами в будущем относительно 2026-03-02
INSERT INTO Lesson ([Date], [Type], ID_Client, ID_Coach, ID_Arena)
VALUES
('20260305', 'Индивидуальное', 1, 1, 1),
('20260305', 'Групповое', 2, 2, 3),
('20260306', 'Прогулка', 3, 3, 6),
('20260306', 'Индивидуальное', 4, 1, 2),
('20260307', 'Тренировка', 5, 4, 4),
('20260307', 'Индивидуальное', 6, 2, 3),
('20260308', 'Подготовка к соревнованиям', 7, 5, 1),
('20260308', 'Групповое', 8, 3, 5),
('20260309', 'Прогулка', 9, 4, 6),
('20260309', 'Индивидуальное', 10, 1, 2),
('20260310', 'Тренировка', 1, 2, 1),
('20260310', 'Индивидуальное', 2, 3, 4);

-- 10. Заполнение таблицы Lesson_Horse (Лошади на занятиях)
INSERT INTO Lesson_Horse (ID_Lesson, ID_Horse)
VALUES
(1, 1), (1, 2), 
(2, 3), (2, 4),
(3, 5),        
(4, 6), (4, 7), 
(5, 8), (5, 9),  
(6, 10),        
(7, 1), (7, 3), 
(8, 4), (8, 5), 
(9, 2),  
(10, 6), (10, 7), (10, 8),
(11, 9), (11, 10),  
(12, 1), (12, 2); 

-- 11. Заполнение таблицы Membership_Lesson (Оплата занятий по абонементу)
INSERT INTO Membership_Lesson (Price, ID_Membership, ID_Lesson)
VALUES
(1000.00, 1, 1),
(900.00, 2, 2),
(850.00, 3, 3),
(1000.00, 4, 4),
(900.00, 5, 5),
(900.00, 6, 6),
(850.00, 7, 7),
(900.00, 8, 8),
(1000.00, 9, 9),
(900.00, 10, 10),
(1000.00, 1, 11),
(900.00, 2, 12);

-- 12. Заполнение таблицы Payment (Платежи) - исправлен формат дат
INSERT INTO Payment (Method_paid, Summa, Payment_date, [Status], ID_Lesson, ID_Membership, ID_Competition)
VALUES
('Карта', 4000.00, '20260201', 'Завершено', NULL, 1, NULL),
('Наличные', 7200.00, '20260115', 'Завершено', NULL, 2, NULL),
('Онлайн', 10200.00, '20251201', 'Завершено', NULL, 3, NULL),
('Карта', 4000.00, '20260210', 'Завершено', NULL, 4, NULL),
('Перевод', 14400.00, '20251101', 'Завершено', NULL, 5, NULL),
('Карта', 7200.00, '20260120', 'Завершено', NULL, 6, NULL),
('Онлайн', 10200.00, '20251015', 'Завершено', NULL, 7, NULL),
('Наличные', 14400.00, '20250901', 'Завершено', NULL, 8, NULL),
('Карта', 4000.00, '20260215', 'Ожидает', NULL, 9, NULL),
('Перевод', 7200.00, '20251210', 'Завершено', NULL, 10, NULL),
('Карта', 1000.00, '20260205', 'Завершено', 1, NULL, NULL),
('Наличные', 900.00, '20260205', 'Завершено', 2, NULL, NULL),
('Онлайн', 2500.00, '20260220', 'Завершено', NULL, NULL, 6);

-- 13. Заполнение таблицы Participation (Участие в соревнованиях)
INSERT INTO Participation (Registration_date, Start_number, Result_place, Score, ID_Competition, ID_Client, ID_Horse)
VALUES
('20260201', 5, 2, 85.5, 6, 3, 1),
('20260201', 8, 4, 78.0, 6, 5, 3),
('20260202', 12, 1, 92.0, 6, 8, 5),
('20251201', 3, 1, 88.5, 7, 2, 4),
('20251201', 7, 3, 75.0, 7, 6, 6),
('20251202', 15, 2, 82.0, 7, 10, 8),
('20260301', 4, NULL, NULL, 3, 1, 2),
('20260301', 9, NULL, NULL, 3, 4, 7),
('20260302', 16, NULL, NULL, 3, 7, 9),
('20260410', 2, NULL, NULL, 1, 3, 1),
('20260410', 10, NULL, NULL, 1, 5, 3),
('20260411', 18, NULL, NULL, 1, 8, 5);

-- 14. Заполнение таблицы Schedule_Arena (Расписание манежей)
INSERT INTO Schedule_Arena (ID_Arena, [Date], Start_time, End_time, [Status], ID_Lesson, ID_Competition)
VALUES
(1, '20260305', '10:00', '11:30', 'Запланировано', 1, NULL),
(3, '20260305', '12:00', '13:30', 'Запланировано', 2, NULL),
(6, '20260306', '15:00', '16:30', 'Запланировано', 3, NULL),
(2, '20260306', '11:00', '12:30', 'Запланировано', 4, NULL),
(4, '20260307', '14:00', '15:30', 'Запланировано', 5, NULL),
(3, '20260307', '16:00', '17:30', 'Запланировано', 6, NULL),
(1, '20260308', '09:00', '10:30', 'Запланировано', 7, NULL),
(5, '20260308', '13:00', '14:30', 'Запланировано', 8, NULL),
(6, '20260309', '11:00', '12:30', 'Запланировано', 9, NULL),
(2, '20260309', '15:00', '16:30', 'Запланировано', 10, NULL),
(1, '20260310', '10:00', '11:30', 'Запланировано', 11, NULL),
(4, '20260310', '14:00', '15:30', 'Запланировано', 12, NULL),
(1, '20260415', '09:00', '18:00', 'Запланировано', NULL, 1),
(3, '20260520', '10:00', '19:00', 'Запланировано', NULL, 2),
(2, '20260310', '09:00', '17:00', 'Запланировано', NULL, 3),
(4, '20260605', '08:00', '20:00', 'Запланировано', NULL, 4),
(5, '20260712', '10:00', '18:00', 'Запланировано', NULL, 5);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lesson_Horse");

            migrationBuilder.DropTable(
                name: "Membership_Lesson");

            migrationBuilder.DropTable(
                name: "Participation");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Schedule_Arena");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Horse");

            migrationBuilder.DropTable(
                name: "Membership");

            migrationBuilder.DropTable(
                name: "Competition");

            migrationBuilder.DropTable(
                name: "Lesson");

            migrationBuilder.DropTable(
                name: "Stall");

            migrationBuilder.DropTable(
                name: "Arena");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Coach");

            migrationBuilder.DropTable(
                name: "Employee");
        }
    }
}
