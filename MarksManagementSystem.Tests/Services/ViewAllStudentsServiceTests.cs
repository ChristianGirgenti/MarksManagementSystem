using FluentAssertions;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Services.Classes;
using Moq;
using MarksManagementSystem.Data.Repositories.Interfaces;

namespace MarksManagementSystem.Tests.Services
{
    public class ViewAllStudentsServiceTests
    {
        public readonly Mock<IStudentRepository> mockStudentRepository;
        public readonly Mock<ICourseStudentRepository> mockCourseStudentRepository;
        public readonly ViewAllStudentsService viewAllStudentsService;

        public ViewAllStudentsServiceTests()
        {
            mockStudentRepository = new Mock<IStudentRepository>();
            mockCourseStudentRepository = new Mock<ICourseStudentRepository>();
            viewAllStudentsService = new ViewAllStudentsService(mockStudentRepository.Object, mockCourseStudentRepository.Object);
        }

        public sealed class Constructor : ViewAllStudentsServiceTests
        {
            [Fact]
            public void GivenANullStudentRepository_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new ViewAllStudentsService(null, mockCourseStudentRepository.Object))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("studentRepository");
            }

            [Fact]
            public void GivenANullCourseStudentRepository_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new ViewAllStudentsService(mockStudentRepository.Object, null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseStudentRepository");
            }
        }

        public sealed class GetAllStudentsViewModel : ViewAllStudentsServiceTests
        {
            [Fact]
            public void GetAllStudentsViewModel_ShouldCallStudentRepositoryGetAll_Once()
            {
                mockStudentRepository.Setup(x => x.GetAll()).Returns(new List<Student>() { new Student { StudentId = 1 } });
                mockCourseStudentRepository.Setup(x => x.GetEnrolledCoursesNameByStudentId(1)).Returns(new List<string>() { "Mathematics" });
                viewAllStudentsService.GetAllStudentsViewModel();
                mockStudentRepository.Verify(x => x.GetAll(), Times.Once);
            }

            [Fact]
            public void GetAllStudentsViewModel_ShouldCallCourseStudentRepositoryGetEnrolledCoursesNameByStudentId_Once()
            {
                mockStudentRepository.Setup(x => x.GetAll()).Returns(new List<Student>() { new Student { StudentId = 1 } });
                mockCourseStudentRepository.Setup(x => x.GetEnrolledCoursesNameByStudentId(1)).Returns(new List<string>() { "Mathematics" });
                viewAllStudentsService.GetAllStudentsViewModel();
                mockStudentRepository.Verify(x => x.GetAll(), Times.Once);
            }
        }

        public sealed class DeleteStudent : ViewAllStudentsServiceTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroStudentId_DeleteStudent_ShouldThrowArgumentOutOfRangeException(int studentId)
            {
                FluentActions.Invoking(() => viewAllStudentsService.DeleteStudent(studentId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(studentId));
            }


            [Theory]
            [InlineData(1)]
            [InlineData(10)]
            [InlineData(100)]
            public void GivenAValidStudentId_DeleteStudent_ShouldCallStudentRepositoryDelete_Once(int studentId)
            {
                viewAllStudentsService.DeleteStudent(studentId);
                mockStudentRepository.Verify(x => x.Delete(studentId), Times.Once);
            }   
        }
    }
}
