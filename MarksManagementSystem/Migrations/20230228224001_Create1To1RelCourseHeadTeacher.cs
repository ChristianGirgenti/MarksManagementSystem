using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarksManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class Create1To1RelCourseHeadTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Courses_HeadTeacherId",
                table: "Courses",
                column: "HeadTeacherId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Teachers_HeadTeacherId",
                table: "Courses",
                column: "HeadTeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Teachers_HeadTeacherId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_HeadTeacherId",
                table: "Courses");
        }
    }
}
