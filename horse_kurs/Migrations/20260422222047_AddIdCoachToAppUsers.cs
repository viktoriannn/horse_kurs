using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace horse_kurs.Migrations
{
    /// <inheritdoc />
    public partial class AddIdCoachToAppUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCoach",
                table: "AppUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_IdCoach",
                table: "AppUsers",
                column: "IdCoach");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_Coach_IdCoach",
                table: "AppUsers",
                column: "IdCoach",
                principalTable: "Coach",
                principalColumn: "ID_Coach");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Coach_IdCoach",
                table: "AppUsers");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_IdCoach",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "IdCoach",
                table: "AppUsers");
        }
    }
}
