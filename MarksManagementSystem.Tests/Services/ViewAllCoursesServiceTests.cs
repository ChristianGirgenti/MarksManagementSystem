using FluentAssertions;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Services.Classes;
using Moq;
using MarksManagementSystem.Data.Repositories.Interfaces;
using MarksManagementSystem.Services.Interfaces;

namespace MarksManagementSystem.Tests.Services
{
    public class ViewAllCoursesServiceTests
    {
        public readonly Mock<ICourseRepository> mockCourseRepository;
        public readonly Mock<ICourseTutorRepository> mockCourseTutorRepository;
        public readonly ViewAllCoursesService viewAllCoursesService;

        public ViewAllCoursesServiceTests()
        {
            mockCourseRepository = new Mock<ICourseRepository>();
            mockCourseTutorRepository = new Mock<ICourseTutorRepository>();
            viewAllCoursesService = new ViewAllCoursesService(mockCourseRepository.Object, mockCourseTutorRepository.Object);
        }

        public sealed class Constructor : ViewAllCoursesServiceTests
        {
            [Fact]
            public void GivenANullCourseRepository_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new ViewAllCoursesService(null, mockCourseTutorRepository.Object))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseRepository");
            }

            [Fact]
            public void GivenANullCourseTutorRepository_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new ViewAllCoursesService(mockCourseRepository.Object, null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseTutorRepository");
            }
        }

        public sealed class GetAllCoursesWithTutors : ViewAllCoursesServiceTests
        {
            [Fact]
            public void GetAllCoursesWithTutors_ShouldCallCourseRepositoryGetAll_Once()
            {
                mockCourseRepository.Setup(x => x.GetAll()).Returns(new List<Course>() { new Course { CourseId = 1 } });
                mockCourseTutorRepository.Setup(x => x.GetUnitLeaderByCourseId(1)).Returns(new Tutor() { TutorId = 1 });
                mockCourseTutorRepository.Setup(x => x.GetOtherTutorsToStringByCourseId(1)).Returns(new List<string>() { "Christian Girgenti" });
                viewAllCoursesService.GetAllCoursesWithTutors();
                mockCourseRepository.Verify(x => x.GetAll(), Times.Once);
            }

            [Fact]
            public void GetAllCoursesWithTutors_ShouldCallCourseTutorRepositoryGetUnitLeaderOfCourse_Once()
            {
                mockCourseRepository.Setup(x => x.GetAll()).Returns(new List<Course>() { new Course { CourseId = 1 } });
                mockCourseTutorRepository.Setup(x => x.GetUnitLeaderByCourseId(1)).Returns(new Tutor() { TutorId = 1 });
                mockCourseTutorRepository.Setup(x => x.GetOtherTutorsToStringByCourseId(1)).Returns(new List<string>() { "Christian Girgenti" });
                viewAllCoursesService.GetAllCoursesWithTutors();
                mockCourseTutorRepository.Verify(x => x.GetUnitLeaderByCourseId(1), Times.Once);
            }

            [Fact]
            public void GetAllCoursesWithTutors_ShouldCallCourseTutorRepositoryGetOtherTutorsOfCourseToString_Once()
            {
                mockCourseRepository.Setup(x => x.GetAll()).Returns(new List<Course>() { new Course { CourseId = 1 } });
                mockCourseTutorRepository.Setup(x => x.GetUnitLeaderByCourseId(1)).Returns(new Tutor() { TutorId = 1 });
                mockCourseTutorRepository.Setup(x => x.GetOtherTutorsToStringByCourseId(1)).Returns(new List<string>() { "Christian Girgenti" });
                viewAllCoursesService.GetAllCoursesWithTutors();
                mockCourseTutorRepository.Verify(x => x.GetOtherTutorsToStringByCourseId(1), Times.Once);
            }
        }

        public sealed class DeleteCourse : ViewAllCoursesServiceTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroCourseId_DeleteCourse_ShouldThrowArgumentOutOfRangeException(int courseId)
            {
                FluentActions.Invoking(() => viewAllCoursesService.DeleteCourse(courseId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }


            [Theory]
            [InlineData(1)]
            [InlineData(10)]
            [InlineData(100)]
            public void GivenValidCourseId_DeleteCourse_ShouldCallCourseRepositoryDelete_Once(int courseId)
            {
                viewAllCoursesService.DeleteCourse(courseId);
                mockCourseRepository.Verify(x => x.Delete(courseId), Times.Once);
            }   
        }
    }
}
