using FluentAssertions;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories.Interfaces;
using MarksManagementSystem.Services.Classes;
using MarksManagementSystem.Services.Interfaces;
using MarksManagementSystem.ViewModel;
using Moq;

namespace MarksManagementSystem.Tests.Services
{
    public class CourseStudentManagementServiceTests
    {
        public readonly Mock<ICourseRepository> mockCourseRepository;
        public readonly Mock<IStudentRepository> mockStudentRepository;
        public readonly Mock<ICourseStudentRepository> mockCourseStudentRepository;

        public readonly CourseStudentManagementService courseStudentManagementService;

        public CourseStudentManagementServiceTests()
        {
            mockCourseRepository = new Mock<ICourseRepository>();
            mockStudentRepository = new Mock<IStudentRepository>();
            mockCourseStudentRepository = new Mock<ICourseStudentRepository>();
            courseStudentManagementService = new CourseStudentManagementService(mockCourseRepository.Object, mockStudentRepository.Object, mockCourseStudentRepository.Object);
        }

        public sealed class Constructor : CourseStudentManagementServiceTests
        {
            [Fact]
            public void GivenANullCourseRepository_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new CourseStudentManagementService(null, mockStudentRepository.Object, mockCourseStudentRepository.Object))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseRepository");
            }

            [Fact]
            public void GivenANullStudentRepository_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new CourseStudentManagementService(mockCourseRepository.Object, null, mockCourseStudentRepository.Object))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("studentRepository");
            }

            [Fact]
            public void GivenANullCourseStudentRepository_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new CourseStudentManagementService(mockCourseRepository.Object, mockStudentRepository.Object, null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseStudentRepository");
            }
        }

        public sealed class GetCourseById : CourseStudentManagementServiceTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]

            public void GivenALessThanOrEqualToZeroCourseId_GetCourseId_ShouldThrowArgumentOutOfRangeException(int courseId)
            {
                FluentActions.Invoking(() => courseStudentManagementService.GetCourseById(courseId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }

            [Fact]
            public void GivenAValidCourseId_GetCourseId_ShouldCallCourseRepositoryGetById_Once()
            {
                courseStudentManagementService.GetCourseById(1);
                mockCourseRepository.Verify(x => x.GetById(1), Times.Once());

            }
        }

        public sealed class GetAllCurrentStudentsInTheCourse : CourseStudentManagementServiceTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]

            public void GivenALessThanOrEqualToZeroCourseId_GetAllCurrentStudentsInTheCourse_ShouldThrowArgumentOutOfRangeException(int courseId)
            {
                FluentActions.Invoking(() => courseStudentManagementService.GetAllCurrentStudentsInTheCourse(courseId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }

            [Fact]
            public void GivenAValidCourseId_GetAllCurrentStudentsInTheCourse_ShouldCallCourseStudentRepositoryGetAllByCourseId_Once()
            {
                courseStudentManagementService.GetAllCurrentStudentsInTheCourse(1);
                mockCourseStudentRepository.Verify(x => x.GetAllByCourseId(1), Times.Once());

            }
        }

        public sealed class PopulateStudentsList : CourseStudentManagementServiceTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]

            public void GivenALessThanOrEqualToZeroCourseId_PopulateStudentsList_ShouldThrowArgumentOutOfRangeException(int courseId)
            {
                FluentActions.Invoking(() => courseStudentManagementService.PopulateStudentsList(courseId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }

            [Fact]
            public void GivenAValidCourseId_PopulateStudentsList_ShouldCallStudentRepositoryGetAll_Once()
            {
                mockStudentRepository.Setup(x => x.GetAll()).Returns(new List<Student>());
                mockCourseStudentRepository.Setup(x => x.GetAll()).Returns(new List<CourseStudent>());
                courseStudentManagementService.PopulateStudentsList(1);
                mockStudentRepository.Verify(x => x.GetAll(), Times.Once());

            }

            [Fact]
            public void GivenAValidCourseId_PopulateStudentsList_ShouldCallCourseStudentRepositoryGetAll_Once()
            {
                mockStudentRepository.Setup(x => x.GetAll()).Returns(new List<Student>());
                mockCourseStudentRepository.Setup(x => x.GetAll()).Returns(new List<CourseStudent>());
                courseStudentManagementService.PopulateStudentsList(1);
                mockCourseStudentRepository.Verify(x => x.GetAll(), Times.Once());
            }
        }

        public sealed class ChangeCourseStudentsRelationship : CourseStudentManagementServiceTests
        {
            [Fact]
            public void GivenANullListOfStudentIds_ChangeCourseStudentsRelationship_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => courseStudentManagementService.ChangeCourseStudentsRelationship(null, new List<CourseStudent>(), new Course()))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("studentIds");
            }

            [Fact]
            public void GivenANullListOfCurrentStudentsInTheCourse_ChangeCourseStudentsRelationship_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => courseStudentManagementService.ChangeCourseStudentsRelationship(new List<string>(), null, new Course()))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("currentStudentsInTheCourse");
            }

            [Fact]
            public void GivenANullCourse_ChangeCourseStudentsRelationship_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => courseStudentManagementService.ChangeCourseStudentsRelationship(new List<string>(), new List<CourseStudent>(), null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("course");
            }
        }

        public sealed class DeleteCourseStudentsRelationship : CourseStudentManagementServiceTests
        {
            [Fact]
            public void GivenANullListOfStudentIds_DeleteCourseStudentsRelationship_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => courseStudentManagementService.DeleteCourseStudentsRelationship(null, new List<CourseStudent>(), 1))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("studentIds");
            }

            [Fact]
            public void GivenANullListOfCurrentStudentsInTheCourse_DeleteCourseStudentsRelationship_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => courseStudentManagementService.DeleteCourseStudentsRelationship(new List<string>(), null, 1))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("currentStudentsInTheCourse");
            }

            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroCourseId_DeleteCourseStudentsRelationship_ShouldThrowArgumentOutOfRangeException(int courseId)
            {
                FluentActions.Invoking(() => courseStudentManagementService.DeleteCourseStudentsRelationship(new List<string>(), new List<CourseStudent>(), courseId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }

            [Fact]
            public void GivenAListOfCurrentStudentsAndListOfStudentIdsDoesNotContainTheCurrentStudentId_DeleteCourseStudentsRelationship_CallsCourseStudentDeleteCourseRelationshipByIDs_Once()
            {
                List<string> studentIds = new List<string> { "1", "2", "3" };
                List<CourseStudent> currentStudents = new List<CourseStudent>()
                {
                    new CourseStudent()
                    {
                        CourseId = 1,
                        StudentId = 4,
                        Mark = 50
                    }
                };
                courseStudentManagementService.DeleteCourseStudentsRelationship(studentIds, currentStudents, 1);
                mockCourseStudentRepository.Verify(x => x.DeleteCourseStudentRelationshipByIds(1, 4), Times.Once);
            }

            [Fact]
            public void GivenAListOfCurrentStudentsAndListOfStudentIdsContainsTheCurrentStudentId_DeleteCourseStudentsRelationship_NeverCallsCourseStudentDeleteCourseRelationshipByIDs()
            {
                List<string> studentIds = new List<string> { "1", "2", "3" };
                List<CourseStudent> currentStudents = new List<CourseStudent>()
                {
                    new CourseStudent()
                    {
                        CourseId = 1,
                        StudentId = 1,
                        Mark = 50
                    }
                };
                courseStudentManagementService.DeleteCourseStudentsRelationship(studentIds, currentStudents, 1);
                mockCourseStudentRepository.Verify(x => x.DeleteCourseStudentRelationshipByIds(1, 1), Times.Never);
            }
        }

        public sealed class AddCourseStudentRelationship : CourseStudentManagementServiceTests
        {
            [Fact]
            public void GivenANullListOfStudentIds_AddCourseStudentRelationship_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => courseStudentManagementService.AddCourseStudentRelationship(null, new List<CourseStudent>(), new Course()))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("studentIds");
            }

            [Fact]
            public void GivenANullListOfCurrentStudentsInTheCourse_AddCourseStudentRelationship_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => courseStudentManagementService.AddCourseStudentRelationship(new List<string>(), null, new Course()))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("currentStudentsInTheCourse");
            }

            [Fact]
            public void GivenANullCourse_AddCourseStudentRelationship_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => courseStudentManagementService.AddCourseStudentRelationship(new List<string>(), new List<CourseStudent>(), null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("course");
            }

            [Fact]
            public void GivenAListOfCurrentStudentsAndListOfStudentIdsDoesNotContainTheCurrentStudentId_AddCourseStudentsRelationship_CallsCourseStudentAdd_Once()
            {
                List<string> studentIds = new List<string> { "1" };
                List<CourseStudent> currentStudents = new List<CourseStudent>()
                {
                    new CourseStudent()
                    {
                        CourseId = 1,
                        StudentId = 4,
                        Mark = 50
                    }
                };
                Student student = new Student()
                {
                    StudentId = 1,
                };

                Course course = new Course()
                {
                    CourseId = 1,
                };

                mockStudentRepository.Setup(x => x.GetById(1)).Returns(student);


                courseStudentManagementService.AddCourseStudentRelationship(studentIds, currentStudents, course);

                mockCourseStudentRepository.Verify(x => x.Add(It.IsAny<CourseStudent>()), Times.Once);
            }

            [Fact]
            public void GivenAListOfCurrentStudentsAndListOfStudentIdsContainsTheCurrentStudentId_AddCourseStudentsRelationship_NeverCallsCourseStudentAdd()
            {
                List<string> studentIds = new List<string> { "1" };
                List<CourseStudent> currentStudents = new List<CourseStudent>()
                {
                    new CourseStudent()
                    {
                        CourseId = 1,
                        StudentId = 1,
                        Mark = 50
                    }
                };
                Student student = new Student()
                {
                    StudentId = 1,
                };

                Course course = new Course()
                {
                    CourseId = 1,
                };

                mockStudentRepository.Setup(x => x.GetById(1)).Returns(student);


                courseStudentManagementService.AddCourseStudentRelationship(studentIds, currentStudents, course);

                mockCourseStudentRepository.Verify(x => x.Add(It.IsAny<CourseStudent>()), Times.Never);
            }

        }

    }
}
