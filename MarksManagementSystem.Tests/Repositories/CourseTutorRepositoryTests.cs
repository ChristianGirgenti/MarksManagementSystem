using MarksManagementSystem.Data.Repositories.Classes;
using FluentAssertions;
using MarksManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using MarksManagementSystem.Data.Repositories.Interfaces;

namespace MarksManagementSystem.UnitTests.Repositories
{
    public class CourseTutorRepositoryTests
    {
        private readonly CourseTutorRepository courseTutorRepository;

        public CourseTutorRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<MarksManagementContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new MarksManagementContext(options);
            courseTutorRepository = new CourseTutorRepository(context);
        }

        public sealed class Constructor : CourseTutorRepositoryTests
        {
            [Fact]
            public void GivenNullMarksManagementContext_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new CourseTutorRepository(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("context");
            }
        }

        public sealed class Add : CourseTutorRepositoryTests
        {
            [Fact]
            public void GivenNullCourseTutor_Add_ShouldThrowArgumentNullException()
            {

                FluentActions.Invoking(() => courseTutorRepository.Add(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseTutor");
            }
        }

        public sealed class DeleteAllOtherTutorsInACourse : CourseTutorRepositoryTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroCourseId_DeleteAllOtherTutorsInACourse_ShouldThrowArgumentOutOfRangeException(int courseId)
            {

                FluentActions.Invoking(() => courseTutorRepository.DeleteAllOtherTutorsInACourse(courseId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }
        }

        public sealed class DeleteCourseUnitLeaderRelationshipByCourseId : CourseTutorRepositoryTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroCourseId_DeleteCourseUnitLeaderRelationshipByCourseId_ShouldThrowArgumentOutOfRangeException(int courseId)
            {

                FluentActions.Invoking(() => courseTutorRepository.DeleteCourseUnitLeaderRelationshipByCourseId(courseId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }

            [Fact]
            public void GivenANullUnitLeaderRelationshipToRemove_DeleteCourseUnitLeaderRelationshipByCourseId_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => courseTutorRepository.DeleteCourseUnitLeaderRelationshipByCourseId(1))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("unitLeaderRelationshipToRemove");
            }
        }

        public sealed class GetAllByTutorId : CourseTutorRepositoryTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroTutorId_GetAllByTutorId_ShouldThrowArgumentOutOfRangeException(int tutorId)
            {

                FluentActions.Invoking(() => courseTutorRepository.GetAllByTutorId(tutorId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(tutorId));
            }
        }

        public sealed class GetByIds : CourseTutorRepositoryTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroCourseId_GetByIds_ShouldThrowArgumentOutOfRangeException(int courseId)
            {

                FluentActions.Invoking(() => courseTutorRepository.GetByIds(courseId, 1))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }

            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroTutorId_GetByIds_ShouldThrowArgumentOutOfRangeException(int tutorId)
            {

                FluentActions.Invoking(() => courseTutorRepository.GetByIds(1, tutorId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(tutorId));
            }

            [Fact]
            public void GivenANullCourseTutorRetrieved_GetByIds_ShouldThrowArgumentNullException()
            {

                FluentActions.Invoking(() => courseTutorRepository.GetByIds(1, 1))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseTutorByIds");
            }
        }

        public sealed class Update : CourseTutorRepositoryTests
        {
            [Fact]
            public void GivenANullCourseTutorToUpdate_Update_ShouldThrowArgumentNullException()
            {

                FluentActions.Invoking(() => courseTutorRepository.Update(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseTutor");
            }
        }

        public sealed class GetUnitLeaderOfCourse : CourseTutorRepositoryTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroCourseId_GetUnitLeaderOfCourse_ShouldThrowArgumentOutOfRangeException(int courseId)
            {

                FluentActions.Invoking(() => courseTutorRepository.GetUnitLeaderOfCourse(courseId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }
        }


        public sealed class GetOtherTutorOfCourseToString : CourseTutorRepositoryTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroCourseId_GetOtherTutorOfCourseToString_ShouldThrowArgumentOutOfRangeException(int courseId)
            {

                FluentActions.Invoking(() => courseTutorRepository.GetOtherTutorsOfCourseToString(courseId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }
        }

    }

}
