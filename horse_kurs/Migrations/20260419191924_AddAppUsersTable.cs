using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace horse_kurs.Migrations
{
    /// <inheritdoc />
    public partial class AddAppUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IdClient = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.IdUser);
                    table.ForeignKey(
                        name: "FK_AppUsers_Client",
                        column: x => x.IdClient,
                        principalTable: "Client",
                        principalColumn: "ID_Client");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_IdClient",
                table: "AppUsers",
                column: "IdClient");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientIdClient = table.Column<int>(type: "int", nullable: true),
                    IdClient = table.Column<int>(type: "int", nullable: true),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClientIdClient",
                table: "Users",
                column: "ClientIdClient");
        }
    }
}
