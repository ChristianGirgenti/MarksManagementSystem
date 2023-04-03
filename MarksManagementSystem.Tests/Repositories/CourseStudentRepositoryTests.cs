using MarksManagementSystem.Data.Repositories.Classes;
using FluentAssertions;
using MarksManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace MarksManagementSystem.UnitTests.Repositories
{
    public class CourseStudentRepositoryTests
    {
        private readonly CourseStudentRepository courseStudentRepository;

        public CourseStudentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<MarksManagementContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new MarksManagementContext(options);
            courseStudentRepository = new CourseStudentRepository(context);
        }

        public sealed class Constructor : CourseStudentRepositoryTests
        {
            [Fact]
            public void GivenNullMarksManagementContext_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new CourseStudentRepository(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("context");
            }
        }

        public sealed class Add : CourseStudentRepositoryTests
        {
            [Fact]
            public void GivenNullCourseStudent_Add_ShouldThrowArgumentNullException()
            {

                FluentActions.Invoking(() => courseStudentRepository.Add(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseStudent");
            }
        }

        public sealed class GetAllByCourseId : CourseStudentRepositoryTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroCourseId_GetAllByCourseId_ShouldThrowArgumentOutOfRangeException(int courseId)
            {

                FluentActions.Invoking(() => courseStudentRepository.GetAllByCourseId(courseId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }
        }


        public sealed class GetByIds : CourseStudentRepositoryTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroCourseId_GetByIds_ShouldThrowArgumentOutOfRangeException(int courseId)
            {

                FluentActions.Invoking(() => courseStudentRepository.GetByIds(courseId, 1))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }

            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroStudentId_GetByIds_ShouldThrowArgumentOutOfRangeException(int studentId)
            {

                FluentActions.Invoking(() => courseStudentRepository.GetByIds(1, studentId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(studentId));
            }

            [Fact]
            public void GivenANullCourseRetrieved_GetByIds_ShouldThrowArgumentNullException()
            {

                FluentActions.Invoking(() => courseStudentRepository.GetByIds(1, 1))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseStudentByIds");
            }
        }


        public sealed class Update : CourseStudentRepositoryTests
        {
            [Fact]
            public void GivenANullCourseToUpdate_Update_ShouldThrowArgumentNullException()
            {

                FluentActions.Invoking(() => courseStudentRepository.Update(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseStudent");
            }
        }

        public sealed class GetAllByStudentId : CourseStudentRepositoryTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroStudentId_GetAllByStudentId_ShouldThrowArgumentOutOfRangeException(int studentId)
            {

                FluentActions.Invoking(() => courseStudentRepository.GetAllByStudentId(studentId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(studentId));
            }
        }

        public sealed class DeleteCourseStudentRelationshipByIds : CourseStudentRepositoryTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroCourseId_DeleteCourseStudentRelationshipByIds_ShouldThrowArgumentOutOfRangeException(int courseId)
            {

                FluentActions.Invoking(() => courseStudentRepository.DeleteCourseStudentRelationshipByIds(courseId, 1))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }

            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroStudentId_DeleteCourseStudentRelationshipByIds_ShouldThrowArgumentOutOfRangeException(int studentId)
            {

                FluentActions.Invoking(() => courseStudentRepository.DeleteCourseStudentRelationshipByIds(1, studentId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(studentId));
            }

            [Fact]
            public void GivenANullCourseStudentRelationshipToRemvoe_DeleteCourseStudentRelationshipByIds_ShouldThrowArgumentNullException()
            {

                FluentActions.Invoking(() => courseStudentRepository.DeleteCourseStudentRelationshipByIds(1, 1))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseStudentRelationshipToRemove");
            }

        }
    }

}
