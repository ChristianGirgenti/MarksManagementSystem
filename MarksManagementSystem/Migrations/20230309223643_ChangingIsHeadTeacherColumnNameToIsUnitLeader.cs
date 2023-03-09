using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarksManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class ChangingIsHeadTeacherColumnNameToIsUnitLeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsHeadTeacher",
                table: "CourseTeachers",
                newName: "IsUnitLeader");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsUnitLeader",
                table: "CourseTeachers",
                newName: "IsHeadTeacher");
        }
    }
}
