using MarksManagementSystem.Data.Repositories.Classes;
using FluentAssertions;
using MarksManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace MarksManagementSystem.UnitTests.Repositories
{
    public class StudentRepositoryTests
    {
        private readonly StudentRepository studentRepository;

        public StudentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<MarksManagementContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new MarksManagementContext(options);
            studentRepository = new StudentRepository(context);
        }

        public sealed class Constructor : StudentRepositoryTests
        {
            [Fact]
            public void GivenNullMarksManagementContext_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new StudentRepository(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("context");
            }
        }

        public sealed class Add : StudentRepositoryTests
        {
            [Fact]
            public void GivenNullStudent_Add_ShouldThrowArgumentNullException()
            {

                FluentActions.Invoking(() => studentRepository.Add(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("student");
            }
        }

        public sealed class Delete : StudentRepositoryTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroStudentId_Delete_ShouldThrowArgumentOutOfRangeException(int studentId)
            {

                FluentActions.Invoking(() => studentRepository.Delete(studentId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(studentId));
            }

            [Fact]
            public void GivenANullStudentToDelete_Delete_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => studentRepository.Delete(5))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("deleteStudent");

            }
        }

        public sealed class GetById : StudentRepositoryTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroStudentId_GetById_ShouldThrowArgumentOutOfRangeException(int studentId)
            {
                FluentActions.Invoking(() => studentRepository.GetById(studentId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(studentId));
            }

            [Fact]
            public void GivenANullStudentToReturn_GetById_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => studentRepository.GetById(5))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("student");

            }
        }

        public sealed class Update : StudentRepositoryTests
        {
            [Fact]
            public void GivenANullStudentToUpdate_Update_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => studentRepository.Update(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("student");
            }
        }

        public sealed class UpdatePasswordByStudentId : StudentRepositoryTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroStudentId_UpdatePasswordByStudentId_ShouldThrowArgumentOutOfRangeException(int studentId)
            {
                FluentActions.Invoking(() => studentRepository.UpdatePasswordByStudentId(studentId, "newPassword"))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(studentId));
            }

            [Theory]
            [InlineData("")]
            [InlineData(null)]
            public void GivenANullOrEmptyNewPassword_UpdatePasswordByStudentId_ShouldThrowArgumentNullException(string newPassword)
            {
                FluentActions.Invoking(() => studentRepository.UpdatePasswordByStudentId(1, newPassword))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName(nameof(newPassword));
            }

            [Fact]
            public void GivenANullStudentToUpdatePassword_UpdatePasswordByStudentId_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => studentRepository.UpdatePasswordByStudentId(1, "newPassword"))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("student");
            }
        }
    }

}
