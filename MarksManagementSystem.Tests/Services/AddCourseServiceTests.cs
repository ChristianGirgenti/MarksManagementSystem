using FluentAssertions;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories.Interfaces;
using MarksManagementSystem.Services.Classes;
using MarksManagementSystem.Services.Interfaces;
using MarksManagementSystem.ViewModel;
using Moq;

namespace MarksManagementSystem.Tests.Services
{
    public class AddCourseServiceTests
    {
        public readonly Mock<ICourseRepository> mockCourseRepository;
        public readonly Mock<ITutorRepository> mockTutorRepository;
        public readonly Mock<ICourseTutorRepository> mockCourseTutorRepository;

        public readonly AddCourseService addCourseService;

        public AddCourseServiceTests()
        {
            mockCourseRepository = new Mock<ICourseRepository>();
            mockTutorRepository = new Mock<ITutorRepository>();
            mockCourseTutorRepository = new Mock<ICourseTutorRepository>();
            addCourseService = new AddCourseService(mockCourseRepository.Object, mockTutorRepository.Object, mockCourseTutorRepository.Object);
        }

        public sealed class Constructor : AddCourseServiceTests
        {
            [Fact]
            public void GivenANullCourseRepository_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new AddCourseService(null, mockTutorRepository.Object, mockCourseTutorRepository.Object))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseRepository");
            }

            [Fact]
            public void GivenANullTutorRepository_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new AddCourseService(mockCourseRepository.Object, null, mockCourseTutorRepository.Object))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("tutorRepository");
            }

            [Fact]
            public void GivenANullCourseTutorRepository_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new AddCourseService(mockCourseRepository.Object, mockTutorRepository.Object, null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseTutorRepository");
            }
        }

        public sealed class GetOtherTutorsInSelectionList : AddCourseServiceTests
        {
            [Fact]
            public void GivenTutors_GetOtherTutorsInSelectionList_ReturnsExpectedList()
            {

                var unitLeaders = new List<Tutor> {
                    new Tutor { TutorId = 1, TutorFirstName = "Christian", TutorLastName = "Girgenti" },
                    new Tutor { TutorId = 2, TutorFirstName = "Milo", TutorLastName = "Girgenti" }
                };

                var nonUnitLeaders = new List<Tutor> {
                    new Tutor { TutorId = 3, TutorFirstName = "Davide", TutorLastName = "Girgenti" },
                    new Tutor { TutorId = 4, TutorFirstName = "John", TutorLastName = "Smith" }
                };

                var mockCourseTutorList = new List<CourseTutor>();
                
                mockCourseTutorRepository.Setup(repo => repo.GetAll()).Returns(mockCourseTutorList);
                mockTutorRepository.Setup(repo => repo.GetAll()).Returns(nonUnitLeaders.ToList());

                var result = addCourseService.GetPossibleUnitLeadersInSelectionList();

                foreach (var unitLeader in unitLeaders)
                {
                    Assert.DoesNotContain(result, item => item.Value == unitLeader.TutorId.ToString());
                }

                foreach (var nonUnitLeader in nonUnitLeaders)
                {
                    Assert.Contains(result, item => item.Value == nonUnitLeader.TutorId.ToString() && item.Text == nonUnitLeader.TutorFirstName + " " + nonUnitLeader.TutorLastName);
                }
            }     
        }

        public sealed class FormatNewCourseValues : AddCourseServiceTests
        {
            [Fact]
            public void GivenANullNewCourse_FormatNewCourseValues_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => addCourseService.FormatNewCourseValues(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("newCourse");
            }

            [Fact]
            public void GivenAValidCourse_FormatNewCourseValues_ShouldCapitaliseTheCourseName()
            {

                var validCourse = new Course
                {
                    CourseId = 1,
                    CourseName = "test",
                    CourseCredits = 30,
                };

                addCourseService.FormatNewCourseValues(validCourse);
                Assert.Equal("Test", validCourse.CourseName);

            }
        }

        public sealed class AddCourse : AddCourseServiceTests
        {
            [Fact]
            public void GivenANullNewCourseViewModel_AddCourse_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => addCourseService.AddCourse(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("newCourseViewModel");
            }

            [Fact]
            public void GivenAValidNewCourseViewModel_AddCourse_ShouldCallAdd_ExactlyOnce()
            {
                var validNewCourseViewModel = new AddEditCourseViewModel()
                {
                    CourseName = "Mathematics",
                    CourseCredits = 30,
                    UnitLeaderId = 1,
                    TutorIds = new List<string>()
                };

                addCourseService.AddCourse(validNewCourseViewModel);
                mockCourseRepository.Verify(x => x.Add(It.IsAny<Course>()), Times.Once);
            }
        }

        public sealed class AddUnitLeaderLinkToCourse : AddCourseServiceTests
        {
            [Fact]
            public void GivenANullNewCourseViewModel_AddUnitLeaderLinkToCourse_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => addCourseService.AddUnitLeaderLinkToCourse(null, new Course()))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("newCourseViewModel");
            }

            [Fact]
            public void GivenANullNewCourse_AddUnitLeaderLinkToCourse_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => addCourseService.AddUnitLeaderLinkToCourse(new AddEditCourseViewModel(), null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("newCourse");
            }

            [Fact]
            public void GivenAValidNewCourseViewModelAndAValidNewCourse_AddUnitLeaderLinkToCourse_ShouldCallGetAll_FromTutorRepository_ExactlyOnce()
            {
                var validNewCourseViewModel = new AddEditCourseViewModel()
                {
                    CourseName = "Mathematics",
                    CourseCredits = 30,
                    UnitLeaderId = 1,
                    TutorIds = new List<string>()
                };

                var newCourse = new Course()
                {
                    CourseName = "Mathematics",
                    CourseCredits = 30,
                };

                var tutors = new List<Tutor> {
                    new Tutor { TutorId = 1, TutorFirstName = "Christian", TutorLastName = "Girgenti" },
                    new Tutor { TutorId = 2, TutorFirstName = "John", TutorLastName = "Smith" }
                };

                mockTutorRepository.Setup(repo => repo.GetAll()).Returns(tutors.ToList());

                addCourseService.AddUnitLeaderLinkToCourse(validNewCourseViewModel, newCourse);
                mockTutorRepository.Verify(x => x.GetAll(), Times.Once);
            }


            [Fact]
            public void GivenAValidNewCourseViewModelAndAValidNewCourse_AddUnitLeaderLinkToCourse_ShouldCallAdd_FromCourseTutorRepository_ExactlyOnce()
            {
                var validNewCourseViewModel = new AddEditCourseViewModel()
                {
                    CourseName = "Mathematics",
                    CourseCredits = 30,
                    UnitLeaderId = 1,
                    TutorIds = new List<string>()
                };

                var newCourse = new Course()
                {
                    CourseName = "Mathematics",
                    CourseCredits = 30,
                };

                var tutors = new List<Tutor> {
                    new Tutor { TutorId = 1, TutorFirstName = "Christian", TutorLastName = "Girgenti" },
                    new Tutor { TutorId = 2, TutorFirstName = "John", TutorLastName = "Smith" }
                };

                mockTutorRepository.Setup(repo => repo.GetAll()).Returns(tutors.ToList());

                addCourseService.AddUnitLeaderLinkToCourse(validNewCourseViewModel, newCourse);
                mockCourseTutorRepository.Verify(x => x.Add(It.IsAny<CourseTutor>()), Times.Once);
            }
        }

    }
}
