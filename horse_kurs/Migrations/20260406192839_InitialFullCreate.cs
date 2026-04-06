using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace horse_kurs.Migrations
{
    /// <inheritdoc />
    public partial class InitialFullCreate : Migration
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
