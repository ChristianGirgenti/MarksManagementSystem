using MarksManagementSystem.Data.Repositories.Classes;
using FluentAssertions;
using MarksManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace MarksManagementSystem.UnitTests.Repositories
{
    public class TutorRepositoryTests
    {
        private readonly TutorRepository tutorRepository;

        public TutorRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<MarksManagementContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new MarksManagementContext(options);
            tutorRepository = new TutorRepository(context);
        }

        public sealed class Constructor : TutorRepositoryTests
        {
            [Fact]
            public void GivenNullMarksManagementContext_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new TutorRepository(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("context");
            }
        }

        public sealed class Add : TutorRepositoryTests
        {
            [Fact]
            public void GivenNullTutor_Add_ShouldThrowArgumentNullException()
            {

                FluentActions.Invoking(() => tutorRepository.Add(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("tutor");
            }
        }

        public sealed class Delete : TutorRepositoryTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroTutorId_Delete_ShouldThrowArgumentOutOfRangeException(int tutorId)
            {

                FluentActions.Invoking(() => tutorRepository.Delete(tutorId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(tutorId));
            }

            [Fact]
            public void GivenANullTutorToDelete_Delete_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => tutorRepository.Delete(5))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("deleteTutor");

            }
        }

        public sealed class GetById : TutorRepositoryTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroTutorId_GetById_ShouldThrowArgumentOutOfRangeException(int tutorId)
            {
                FluentActions.Invoking(() => tutorRepository.GetById(tutorId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(tutorId));
            }

            [Fact]
            public void GivenANullTutorToReturn_GetById_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => tutorRepository.GetById(5))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("tutor");

            }
        }

        public sealed class Update : TutorRepositoryTests
        {
            [Fact]
            public void GivenANullTutorToUpdate_Update_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => tutorRepository.Update(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("tutor");
            }
        }

        public sealed class UpdatePasswordByTutorId : TutorRepositoryTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroTutorId_UpdatePasswordByStudentId_ShouldThrowArgumentOutOfRangeException(int tutorId)
            {
                FluentActions.Invoking(() => tutorRepository.UpdatePasswordByTutorId(tutorId, "newPassword"))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(tutorId));
            }

            [Theory]
            [InlineData("")]
            [InlineData(null)]
            public void GivenANullOrEmptyNewPassword_UpdatePasswordByTutorId_ShouldThrowArgumentNullException(string newPassword)
            {
                FluentActions.Invoking(() => tutorRepository.UpdatePasswordByTutorId(1, newPassword))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName(nameof(newPassword));
            }

            [Fact]
            public void GivenANullTutorToUpdatePassword_UpdatePasswordByTutorId_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => tutorRepository.UpdatePasswordByTutorId(1, "newPassword"))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("tutor");
            }
        }
    }

}
