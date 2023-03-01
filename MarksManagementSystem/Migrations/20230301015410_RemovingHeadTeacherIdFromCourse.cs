using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarksManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class RemovingHeadTeacherIdFromCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeadTeacherId",
                table: "Courses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HeadTeacherId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
