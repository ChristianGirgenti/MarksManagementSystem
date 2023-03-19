using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarksManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordSaltInTutorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Tutor",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Tutor");
        }
    }
}
