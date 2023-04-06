using FluentAssertions;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Services.Classes;
using Moq;
using MarksManagementSystem.Data.Repositories.Interfaces;

namespace MarksManagementSystem.Tests.Services
{
    public class ViewCourseServiceTests
    {
        public readonly Mock<ICourseRepository> mockCourseRepository;
        public readonly Mock<ICourseTutorRepository> mockCourseTutorRepository;
        public readonly Mock<ICourseStudentRepository> mockCourseStudentRepository;
        public readonly Mock<IStudentRepository> mockStudentRepository;

        public readonly ViewCourseService viewCourseService;

        public ViewCourseServiceTests()
        {
           mockCourseRepository = new Mock<ICourseRepository>();
           mockCourseTutorRepository = new Mock<ICourseTutorRepository>();
           mockCourseStudentRepository = new Mock<ICourseStudentRepository>();
           mockStudentRepository = new Mock<IStudentRepository>();
           viewCourseService = new ViewCourseService(mockCourseRepository.Object, mockCourseTutorRepository.Object, mockCourseStudentRepository.Object, mockStudentRepository.Object);
        }

        public sealed class Constructor : ViewCourseServiceTests
        {
            [Fact]
            public void GivenANullCourseRepository_WhenConstructing_ThenThrowArgumentNullException()
            {
                FluentActions.Invoking(() => new ViewCourseService(null, mockCourseTutorRepository.Object, mockCourseStudentRepository.Object, mockStudentRepository.Object))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseRepository");
            }

            [Fact]
            public void GivenANullCourseTutorRepository_WhenConstructing_ThenThrowArgumentNullException()
            {
                FluentActions.Invoking(() => new ViewCourseService(mockCourseRepository.Object, null, mockCourseStudentRepository.Object, mockStudentRepository.Object))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseTutorRepository");
            }

            [Fact]
            public void GivenANullCourseStudentRepository_WhenConstructing_ThenThrowArgumentNullException()
            {
                FluentActions.Invoking(() => new ViewCourseService(mockCourseRepository.Object, mockCourseTutorRepository.Object, null, mockStudentRepository.Object))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseStudentRepository");
            }

            [Fact]
            public void GivenANullStudentRepository_WhenConstructing_ThenThrowArgumentNullException()
            {
                FluentActions.Invoking(() => new ViewCourseService(mockCourseRepository.Object, mockCourseTutorRepository.Object, mockCourseStudentRepository.Object, null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("studentRepository");
            }
        }

        public sealed class GetCourseById : ViewCourseServiceTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroCourseId_GetCourseById_ShouldThrowArgumentOutOfRangeException(int courseId)
            {
                FluentActions.Invoking(() => viewCourseService.GetCourseById(courseId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }


            [Theory]
            [InlineData(1)]
            [InlineData(10)]
            [InlineData(100)]
            public void GivenValidCourseId_GetCourseById_ShouldCallCourseRepositoryGetById_Once(int courseId)
            {
                viewCourseService.GetCourseById(courseId);
                mockCourseRepository.Verify(x => x.GetById(courseId), Times.Once);
            }
        }

        public sealed class GetUnitLeaderOfCourse : ViewCourseServiceTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroCourseId_GetUnitLeaderOfCourse_ShouldThrowArgumentOutOfRangeException(int courseId)
            {
                FluentActions.Invoking(() => viewCourseService.GetUnitLeaderOfCourse(courseId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }


            [Theory]
            [InlineData(1)]
            [InlineData(10)]
            [InlineData(100)]
            public void GivenValidCourseId_GetUnitLeaderOfCourse_ShouldCallCourseTutorRepositoryGetUnitLeaderOfCourse_Once(int courseId)
            {
                viewCourseService.GetUnitLeaderOfCourse(courseId);
                mockCourseTutorRepository.Verify(x => x.GetUnitLeaderOfCourse(courseId), Times.Once);
            }
        }

        public sealed class UpdateMarks : ViewCourseServiceTests
        {
            [Fact]
            public void GivenANullCourse_UpdateMarks_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => viewCourseService.UpdateMarks(null, "100", 1))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("thisCourse");
            }

            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroStudentId_UpdateMarks_ShouldThrowArgumentOutOfRangeException(int studentId)
            {
                FluentActions.Invoking(() => viewCourseService.UpdateMarks(new Course(), "100", studentId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(studentId));
            }

            [Fact]
            public void GivenValidParameters_UpdateMarks_ShouldCallStudentRepositoryGetById_Once()
            {
                mockStudentRepository.Setup(x => x.GetById(1)).Returns(new Student() { StudentId = 1 });
                mockCourseStudentRepository.Setup(x => x.GetByIds(1, 1)).Returns(new CourseStudent());

                viewCourseService.UpdateMarks(new Course() {  CourseId = 1 }, "100", 1);
                mockStudentRepository.Verify(x => x.GetById(1), Times.Once);
            }

            [Fact]
            public void GivenValidParameters_UpdateMarks_ShouldCallCourseStudentRepositoryGetByIds_Once()
            {
                mockStudentRepository.Setup(x => x.GetById(1)).Returns(new Student() { StudentId = 1 });
                mockCourseStudentRepository.Setup(x => x.GetByIds(1, 1)).Returns(new CourseStudent());

                viewCourseService.UpdateMarks(new Course() { CourseId = 1 }, "100", 1);
                mockCourseStudentRepository.Verify(x => x.GetByIds(1, 1), Times.Once);
            }

            [Fact]
            public void GivenValidParameters_UpdateMarks_ShouldCallCourseStudentRepositoryUpdate_Once_WhenCourseStudentIsNotNull()
            {
                mockStudentRepository.Setup(x => x.GetById(1)).Returns(new Student() { StudentId = 1 });
                mockCourseStudentRepository.Setup(x => x.GetByIds(1, 1)).Returns(new CourseStudent());

                viewCourseService.UpdateMarks(new Course() { CourseId = 1 }, "100", 1);
                mockCourseStudentRepository.Verify(x => x.Update(It.IsAny<CourseStudent>()), Times.Once);
            }
        }

        public sealed class GetAllStudentsEnrolled : ViewCourseServiceTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroCourseId_GetAllStudentsEnrolled_ShouldThrowArgumentOutOfRangeException(int courseId)
            {
                FluentActions.Invoking(() => viewCourseService.GetAllStudentEnrolled(courseId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }

            [Theory]
            [InlineData(1)]
            [InlineData(10)]
            [InlineData(100)]
            public void GivenAValidCourseId_GetAllStudentsEnrolled_ShouldCallCourseStudentRepositoryGetAllByCourseId_Once(int courseId)
            {
                mockCourseStudentRepository.Setup(x => x.GetAllByCourseId(courseId)).Returns(new List<CourseStudent>());
                viewCourseService.GetAllStudentEnrolled(courseId);
                mockCourseStudentRepository.Verify(x => x.GetAllByCourseId(courseId), Times.Once);
            }
        }

    }
}
