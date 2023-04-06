using FluentAssertions;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Services.Classes;
using Moq;
using MarksManagementSystem.Data.Repositories.Interfaces;
using System.Security.Claims;

namespace MarksManagementSystem.Tests.Services
{
    public class ViewAllTutorsServiceTests
    {
        public readonly Mock<ITutorRepository> mockTutorRepository;
        public readonly Mock<ICourseTutorRepository> mockCourseTutorRepository;
        public readonly ViewAllTutorsService viewAllTutorsService;

        public ViewAllTutorsServiceTests()
        {
            mockTutorRepository = new Mock<ITutorRepository>();
            mockCourseTutorRepository = new Mock<ICourseTutorRepository>();
            viewAllTutorsService = new ViewAllTutorsService(mockTutorRepository.Object, mockCourseTutorRepository.Object);
        }

        public sealed class Constructor : ViewAllTutorsServiceTests
        {
            [Fact]
            public void GivenANullTutorRepository_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new ViewAllTutorsService(null, mockCourseTutorRepository.Object))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("tutorRepository");
            }

            [Fact]
            public void GivenANullCourseTutorRepository_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new ViewAllTutorsService(mockTutorRepository.Object, null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseTutorRepository");
            }
        }

        public sealed class GetAllTutorsViewModel : ViewAllTutorsServiceTests
        {
            [Fact]
            public void GetAllTutorsViewModel_ShouldCallTutorRepositoryGetAll_Once()
            {
                mockTutorRepository.Setup(x => x.GetAll()).Returns(new List<Tutor>() { new Tutor { TutorId = 1 } });
                mockCourseTutorRepository.Setup(x => x.GetAll()).Returns(new List<CourseTutor>());
                viewAllTutorsService.GetAllTutorsViewModel();
                mockTutorRepository.Verify(x => x.GetAll(), Times.Once);
            }

            [Fact]
            public void GetAllTutorsViewModel_ShouldCallCourseTutorRepositoryGetAll_Twice()
            {
                mockTutorRepository.Setup(x => x.GetAll()).Returns(new List<Tutor>() { new Tutor { TutorId = 1 } });
                mockCourseTutorRepository.Setup(x => x.GetAll()).Returns(new List<CourseTutor>());
                viewAllTutorsService.GetAllTutorsViewModel();
                mockCourseTutorRepository.Verify(x => x.GetAll(), Times.Exactly(2));
            }
        }

        public sealed class IsTutorUnitLeader : ViewAllTutorsServiceTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroTutorId_IsTutorUnitLeader_ShouldThrowArgumentOutOfRangeException(int tutorId)
            {
                FluentActions.Invoking(() => viewAllTutorsService.IsTutorUnitLeader(tutorId))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(tutorId));
            }


            [Theory]
            [InlineData(1)]
            [InlineData(10)]
            [InlineData(100)]
            public void GivenAValidTutorId_IsTutorUnitLeader_ShouldCallCourseTutorRepositoryGetAll_Once(int tutorId)
            {
                mockCourseTutorRepository.Setup(x => x.GetAll()).Returns(new List<CourseTutor>());
                viewAllTutorsService.IsTutorUnitLeader(tutorId);
                mockCourseTutorRepository.Verify(x => x.GetAll(), Times.Once);
            }
        }

        public sealed class DeleteTutor : ViewAllTutorsServiceTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            public void GivenALessThanOrEqualToZeroTutorId_DeleteTutor_ShouldThrowArgumentOutOfRangeException(int tutorId)
            {
                List<Claim> claims = new();
                FluentActions.Invoking(() => viewAllTutorsService.DeleteTutor(tutorId, claims))
                    .Should()
                    .ThrowExactly<ArgumentOutOfRangeException>()
                    .WithParameterName(nameof(tutorId));
            }

            [Fact]
            public void GivenANullIEnumerableOfClaims_DeleteTutor_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => viewAllTutorsService.DeleteTutor(1, null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("claims");
            }

            [Theory]
            [InlineData(1)]
            [InlineData(10)]
            [InlineData(100)]
            public void GivenAValidTutorId_DeleteTutor_ShouldCallTutorReposityGetById_Once(int tutorId)
            {
                Claim claim = new("Email", "differentEmail@uni.co.uk");
                List<Claim> claims = new() { claim };
                Tutor tutorToDelete = new() { TutorEmail = "christian.girgenti@uni.co.uk" };

                mockTutorRepository.Setup(x => x.GetById(tutorId)).Returns(tutorToDelete);
                viewAllTutorsService.DeleteTutor(tutorId, claims);
                mockTutorRepository.Verify(x => x.GetById(tutorId), Times.Once);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(10)]
            [InlineData(100)]
            public void GivenAValidTutorId_DeleteTutor_ShouldCallTutorReposityDelete_Once_AndAlsoReturnTrue_WhenTutorEmailIsDifferentFromCurrentUserEmail(int tutorId)
            {
                Claim claim = new("Email", "differentEmail@uni.co.uk");
                List<Claim> claims = new() { claim };
                Tutor tutorToDelete = new() { TutorEmail = "christian.girgenti@uni.co.uk" };

                mockTutorRepository.Setup(x => x.GetById(tutorId)).Returns(tutorToDelete);

                var actual = viewAllTutorsService.DeleteTutor(tutorId, claims);
                mockTutorRepository.Verify(x => x.Delete(tutorId), Times.Once);
                Assert.True(actual);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(10)]
            [InlineData(100)]
            public void GivenAValidTutorId_DeleteTutor_ShouldReturnFalse_WhenTutorEmailIsTheSameOfTheCurrentUserEmail(int tutorId)
            {
                Claim claim = new("Email", "christian.girgenti@uni.co.uk");
                List<Claim> claims = new() { claim };
                Tutor tutorToDelete = new() { TutorEmail = "christian.girgenti@uni.co.uk" };

                mockTutorRepository.Setup(x => x.GetById(tutorId)).Returns(tutorToDelete);

                var actual = viewAllTutorsService.DeleteTutor(tutorId, claims);
                Assert.False(actual);
            }
        }
    }
}
