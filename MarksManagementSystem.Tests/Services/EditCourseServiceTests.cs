using FluentAssertions;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories.Interfaces;
using MarksManagementSystem.Services.Classes;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;

namespace MarksManagementSystem.Tests.Services
{
    public class EditCourseServiceTests
    {
        public readonly Mock<ICourseRepository> mockCourseRepository;
        public readonly Mock<ITutorRepository> mockTutorRepository;
        public readonly Mock<ICourseTutorRepository> mockCourseTutorRepository;


        public readonly EditCourseService editCourseService;

        public EditCourseServiceTests()
        {
            mockCourseRepository = new Mock<ICourseRepository>();
            mockTutorRepository = new Mock<ITutorRepository>();
            mockCourseTutorRepository = new Mock<ICourseTutorRepository>();
            editCourseService = new EditCourseService(mockCourseRepository.Object, mockTutorRepository.Object, mockCourseTutorRepository.Object);
        }

        public sealed class Constructor : EditCourseServiceTests
        {
            [Fact]
            public void GivenANullCourseRepository_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new EditCourseService(null, mockTutorRepository.Object, mockCourseTutorRepository.Object))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseRepository");
            }

            [Fact]
            public void GivenANullStudentRepository_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new EditCourseService(mockCourseRepository.Object, null, mockCourseTutorRepository.Object))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("tutorRepository");
            }

            [Fact]
            public void GivenANullCourseStudentRepository_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new EditCourseService(mockCourseRepository.Object, mockTutorRepository.Object, null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseTutorRepository");
            }
        }

        public sealed class GetCourseToEditById : EditCourseServiceTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroCourseId_GetCourseToEditById_ShouldThrowArgumentOutOfRangeException(int courseId)
            {

                FluentActions.Invoking(() => editCourseService.GetCourseToEditById(courseId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }

            [Theory]
            [InlineData(1)]
            [InlineData(10)]
            [InlineData(100)]
            public void GivenAValidCourseId_GetCourseToEditById_ShouldCallCourseRepositoryGetById_Once(int courseId)
            {
                editCourseService.GetCourseToEditById(courseId);
                mockCourseRepository.Verify(x => x.GetById(courseId), Times.Once());
            }
        }

        public sealed class GetAllTheCourseTutors : EditCourseServiceTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroCourseId_GetAllTheCourseTutors_ShouldThrowArgumentOutOfRangeException(int courseId)
            {
                FluentActions.Invoking(() => editCourseService.GetAllTheCourseTutors(courseId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }
        }

        public sealed class GetUnitLeaderId : EditCourseServiceTests
        {
            [Fact]
            public void GivenNullAllCourseTutors_GetUnitLeaderId_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => editCourseService.GetUnitLeaderId(null, 1))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("allCourseTutors");
            }

            [Fact]
            public void GivenValidAllCourseTutors_GetUnitLeaderId_ShouldReturnTheUnitLeaderId()
            {
                CourseTutor courseTutor = new CourseTutor
                {
                    CourseId = 1,
                    TutorId = 1,
                    IsUnitLeader = true
                };

                List<CourseTutor> courseTutors = new List<CourseTutor>()
                {
                    courseTutor
                };

                var unitLeaderId = editCourseService.GetUnitLeaderId(courseTutors, 1);
                Assert.Equal(courseTutor.TutorId, unitLeaderId);    
            }
        }

        public sealed class ShowPossibleUnitLeaderInSelectionList : EditCourseServiceTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroUnitLeaderId_ShowPossibleUnitLeaderInSelectionList_ShouldThrowArgumentOutOfRangeException(int unitLeaderId)
            {
                FluentActions.Invoking(() => editCourseService.ShowPossibleUnitLeaderInSelectionList(unitLeaderId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(unitLeaderId));
            }

            [Fact]
            public void GivenValidUnitLeaderId_ShowPossibleUnitLeaderInSelectionList_ShouldReturnTheExpectedPossibleUnitLeaders()
            {
                List<CourseTutor> unitLeader = new List<CourseTutor>()
                {
                    new CourseTutor()
                    {
                        CourseId = 1,
                        TutorId = 1,
                        IsUnitLeader = true,
                    }
                };

                List<Tutor> tutors = new List<Tutor>()
                {
                    new Tutor()
                    {
                        TutorId = 2,
                        TutorFirstName = "Christian",
                        TutorLastName = "Girgenti",
                    }
                };

                List<SelectListItem>? expected = new List<SelectListItem>()
                {
                    new SelectListItem
                    {
                        Value = "2",
                        Text = "Christian Girgenti",
                        Disabled = false,
                        Group = null,
                        Selected = false
                    }
                };

                mockCourseTutorRepository.Setup(x => x.GetAll()).Returns(unitLeader);
                mockTutorRepository.Setup(x => x.GetAll()).Returns(tutors);

                var possibleUnitLeader = editCourseService.ShowPossibleUnitLeaderInSelectionList(1);
                Assert.Equal(expected.ElementAt(0).Value, possibleUnitLeader.ElementAt(0).Value);
                Assert.Equal(expected.ElementAt(0).Text, possibleUnitLeader.ElementAt(0).Text);
            }
        }

        public sealed class PopulateOtherTutors : EditCourseServiceTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroUnitLeaderId_PopulateOtherTutors_ShouldThrowArgumentOutOfRangeException(int unitLeaderId)
            {
                FluentActions.Invoking(() => editCourseService.PopulateOtherTutors(unitLeaderId, 1))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(unitLeaderId));
            }

            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroCourseId_PopulateOtherTutors_ShouldThrowArgumentOutOfRangeException(int courseId)
            {
                FluentActions.Invoking(() => editCourseService.PopulateOtherTutors(1, courseId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }

            [Fact]
            public void GivenValidUnitLeaderIdAndValidCourseId_PopulateOtherTutors_ShouldReturnTheExpectedOtherTutors()
            {
                List<CourseTutor> currentOtherTutors = new List<CourseTutor>()
                {
                    new CourseTutor()
                    {
                        CourseId = 1,
                        TutorId = 3,
                        IsUnitLeader = false,
                    }
                };

                List<Tutor> otherTutors = new List<Tutor>()
                {
                    new Tutor()
                    {
                        TutorId = 2,
                        TutorFirstName = "Christian",
                        TutorLastName = "Girgenti",
                    },
                                        new Tutor()
                    {
                        TutorId = 1,
                        TutorFirstName = "Davide",
                        TutorLastName = "Girgenti",
                    }
                };

                List<SelectListItem>? expected = new List<SelectListItem>()
                {
                    new SelectListItem
                    {
                        Value = "2",
                        Text = "Christian Girgenti",
                        Selected = false
                    },
                    new SelectListItem
                    {
                        Value = "1",
                        Text = "Davide Girgenti",
                        Selected = false
                    }
                };

                mockCourseTutorRepository.Setup(x => x.GetAll()).Returns(currentOtherTutors);
                mockTutorRepository.Setup(x => x.GetAll()).Returns(otherTutors);

                var otherTutorsReturned = editCourseService.PopulateOtherTutors(4, 1);
                Assert.Equal(expected.ElementAt(0).Value, otherTutorsReturned.ElementAt(0).Value);
                Assert.Equal(expected.ElementAt(0).Text, otherTutorsReturned.ElementAt(0).Text);
                Assert.Equal(expected.ElementAt(0).Selected, otherTutorsReturned.ElementAt(0).Selected);
                Assert.Equal(expected.ElementAt(1).Text, otherTutorsReturned.ElementAt(1).Text);
                Assert.Equal(expected.ElementAt(1).Value, otherTutorsReturned.ElementAt(1).Value);
                Assert.Equal(expected.ElementAt(1).Selected, otherTutorsReturned.ElementAt(1).Selected);
            }
        }

        public sealed class FormatNewCourseValue : EditCourseServiceTests
        {
            [Fact]
            public void GivenANullAddEditCourseViewModel_FormatNewCourseValue_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => editCourseService.FormatNewCourseValues(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("editCourseViewModel");
            }

            [Fact]
            public void GivenAValidAddEditCourseViewModel_FormatNewCourseValue_ShouldReturnTheViewModelWithCapitalisedCourseName()
            {
                AddEditCourseViewModel addEditCourseViewModel = new AddEditCourseViewModel()
                {
                    CourseName = "mathematics",
                    CourseCredits = 30,
                    TutorIds = new List<string> { "1", "2" },
                    UnitLeaderId = 1
                };

                var expected = "Mathematics";
                var actual = editCourseService.FormatNewCourseValues(addEditCourseViewModel);

                Assert.Equal(expected, actual.CourseName);
            }
        }

        public sealed class EditCourse : EditCourseServiceTests
        {

            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroCourseId_EditCourse_ShouldThrowArgumentOutOfRangeException(int courseId)
            {
                FluentActions.Invoking(() => editCourseService.EditCourse(courseId, new AddEditCourseViewModel()))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(courseId));
            }

            [Fact]
            public void GivenANullAddEditCourseViewModel_EditCourse_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => editCourseService.EditCourse(1, null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("editCourseViewModel");
            }

            [Fact]
            public void GivenValidParameters_EditCourse_ShouldCallCourseRepositoryUpdate_Once()
            {
                AddEditCourseViewModel addEditCourseViewModel = new()
                {
                    CourseName = "Mathematics",
                    CourseCredits = 30,
                    TutorIds = new List<string> { "1", "2" },
                    UnitLeaderId = 1
                };

                editCourseService.EditCourse(1, addEditCourseViewModel);
                mockCourseRepository.Verify(x => x.Update(It.IsAny<Course>()), Times.Once);
            }

        }


        public sealed class ChangeTutorCourseRelationships : EditCourseServiceTests
        {
            [Fact]
            public void GivenANullAddEditCourseViewModel_ChangeTutorCourseRelationships_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => editCourseService.ChangeTutorCourseRelationships(null, new Course(), new List<string>()))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("editCourseViewModel");
            }

            [Fact]
            public void GivenANullCourseToEdit_ChangeTutorCourseRelationships_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => editCourseService.ChangeTutorCourseRelationships(new AddEditCourseViewModel(), null, new List<string>()))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseEdited");
            }

            [Fact]
            public void GivenValidParameters_ChangeTutorCourseRelationship_ShouldCall_CourseTutorRepositoryDeleteCourseUnitLeaderRelationshipByCourseId_Once()
            {
                mockTutorRepository.Setup(x => x.GetAll()).Returns(new List<Tutor>());
                editCourseService.ChangeTutorCourseRelationships(new AddEditCourseViewModel(), new Course(), new List<string>());
                mockCourseTutorRepository.Verify(x => x.DeleteCourseUnitLeaderRelationshipByCourseId(It.IsAny<int>()), Times.Once);
            }

            [Fact]
            public void GivenValidParameters_ChangeTutorCourseRelationship_ShouldCall_CourseTutorRepositoryDeleteAllOtherTutorsInACourse_Once()
            {
                mockTutorRepository.Setup(x => x.GetAll()).Returns(new List<Tutor>());
                editCourseService.ChangeTutorCourseRelationships(new AddEditCourseViewModel(), new Course(), new List<string>());
                mockCourseTutorRepository.Verify(x => x.DeleteAllOtherTutorsInACourse(It.IsAny<int>()), Times.Once);
            }

            [Fact]
            public void GivenValidParameters_ChangeTutorCourseRelationship_ShouldCall_TutorRepositoryGetAll_Once()
            {
                mockTutorRepository.Setup(x => x.GetAll()).Returns(new List<Tutor>());
                editCourseService.ChangeTutorCourseRelationships(new AddEditCourseViewModel(), new Course(), new List<string>());
                mockTutorRepository.Verify(x => x.GetAll(), Times.Once);
            }

        }

        public sealed class AddOtherTutorsRelationship : EditCourseServiceTests
        {
            [Fact]
            public void GivenANullListOfOtherTutors_AddOtherTutorsRelationship_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => editCourseService.AddOtherTutorsRelationship(null, new Course(), new AddEditCourseViewModel()))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("otherTutors");
            }

            [Fact]
            public void GivenANullCourseToEdit_AddOtherTutorsRelationship_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => editCourseService.AddOtherTutorsRelationship(new List<string>(), null, new AddEditCourseViewModel()))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseEdited");
            }

            [Fact]
            public void GivenANullAddEditCourseViewModel_AddOtherTutorsRelationship_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => editCourseService.AddOtherTutorsRelationship(new List<string>(), new Course(), null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("editCourseViewModel");
            }

            [Fact]
            public void GivenValidParameters_AddOtherTutorsRelationship_ShouldCall_TutorRepositoryGetById_AtLeastOnce()
            {
                var tutor = new Tutor()
                {
                    TutorId = 1
                };

                var otherTutors = new List<Tutor>()
                {
                    tutor
                };

                var addEditCourseViewModel = new AddEditCourseViewModel()
                {
                    UnitLeaderId = 2
                };

                var otherTutorsString = new List<string>() { "1" };


                mockTutorRepository.Setup(x => x.GetById(1)).Returns(tutor);

                editCourseService.AddOtherTutorsRelationship(otherTutorsString, new Course(), addEditCourseViewModel);
                mockTutorRepository.Verify(x => x.GetById(1), Times.AtLeastOnce());
            }

            [Fact]
            public void GivenValidParameters_AddOtherTutorsRelationship_ShouldCall_CourseTutorRepositoryAdd_AtLeastOnce()
            {
                var tutor = new Tutor()
                {
                    TutorId = 1
                };

                var otherTutors = new List<Tutor>()
                {
                    tutor
                };

                var addEditCourseViewModel = new AddEditCourseViewModel()
                {
                    UnitLeaderId = 2
                };

                var otherTutorsString = new List<string>() { "1" };

                mockTutorRepository.Setup(x => x.GetById(1)).Returns(tutor);

                editCourseService.AddOtherTutorsRelationship(otherTutorsString, new Course(), addEditCourseViewModel);
                mockCourseTutorRepository.Verify(x => x.Add(It.IsAny<CourseTutor>()), Times.AtLeastOnce());
            }

        }

        public sealed class AddUnitLeaderRelationship : EditCourseServiceTests
        {
            [Fact]
            public void GivenANullNewUnitLeader_AddUnitLeaderRelationship_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => editCourseService.AddUnitLeaderRelationship(null, new Course()))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("newUnitLeader");
            }

            [Fact]
            public void GivenANullCourseToEdit_AddUnitLeaderRelationship_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => editCourseService.AddUnitLeaderRelationship(new Tutor(), null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseEdited");
            } 

            [Fact]
            public void GivenValidParameters_AddUnitLeaderRelationship_ShouldCall_CourseTutorRepositoryAdd_AtLeastOnce()
            {
                editCourseService.AddUnitLeaderRelationship(new Tutor(), new Course());
                mockCourseTutorRepository.Verify(x => x.Add(It.IsAny<CourseTutor>()), Times.AtLeastOnce());
            }
        }
    }
}
