using Xunit;
using MarksManagementSystem.Data.Repositories.Classes;
using FluentAssertions;
using Moq;
using MarksManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using MarksManagementSystem.Data.Models;

namespace MarksManagementSystem.UnitTests.Repositories
{
    public class CourseRepositoryTests
    {
        private readonly CourseRepository courseRepository;

        public CourseRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<MarksManagementContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new MarksManagementContext(options);
            courseRepository = new CourseRepository(context);
        }

        public sealed class Constructor : CourseRepositoryTests
        {
            [Fact]
            public void GivenNullMarksManagementContext_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new CourseRepository(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("context");
            }
        }

        public sealed class Add : CourseRepositoryTests
        {
            [Fact]
            public void GivenNullCourse_AddCourse_ShouldThrowArgumentNullException()
            {

                FluentActions.Invoking(() => courseRepository.Add(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("course");
            }

            [Fact]
            public void GivenAValidCourse_Add_ShouldSaveTheCourseInTheDatabase()
            {
                var course = new Course()
                {
                    CourseName = "Mathematics",
                    CourseCredits = 30,
                };
                courseRepository.Add(course);

                var savedCourse = courseRepository.GetById(course.CourseId);

                savedCourse.Should().BeEquivalentTo(course);
            }
        }

        public sealed class Delete : CourseRepositoryTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroCourseId_Delete_ShouldThrowArgumentOutOfRangeException(int courseId)
            {

                FluentActions.Invoking(() => courseRepository.Delete(courseId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }

            [Fact]
            public void GivenANullCourseToDelete_Delete_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => courseRepository.Delete(1))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseId");

            }
        }

        public sealed class GetById : CourseRepositoryTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroCourseId_GetById_ShouldThrowArgumentOutOfRangeException(int courseId)
            {
                FluentActions.Invoking(() => courseRepository.GetById(courseId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }

            [Fact]
            public void GivenANullCourseToReturn_GetById_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => courseRepository.GetById(1))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseId");

            }
        }

        public sealed class Update : CourseRepositoryTests
        {
            [Fact]
            public void GivenANullCourseToUpdate_Update_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => courseRepository.Update(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("course");
            }
        }
    }

}
