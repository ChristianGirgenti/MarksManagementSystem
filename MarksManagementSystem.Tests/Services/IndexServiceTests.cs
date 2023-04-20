using FluentAssertions;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories.Interfaces;
using MarksManagementSystem.Helpers;
using MarksManagementSystem.Services.Classes;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using System.Security.Cryptography;


namespace MarksManagementSystem.Tests.Services
{
    public class IndexServiceTests
    {
        public readonly Mock<ICourseTutorRepository> mockCourseTutorRepository;
        public readonly Mock<ICourseStudentRepository> mockCourseStudentRepository;
        public readonly Mock<IPasswordCreator> mockPasswordCreator;
        public readonly Mock<HttpContext> mockHttpContext;

        public readonly IndexService indexService;

        public IndexServiceTests()
        {
            mockCourseTutorRepository = new Mock<ICourseTutorRepository>();
            mockCourseStudentRepository = new Mock<ICourseStudentRepository>();
            mockPasswordCreator = new Mock<IPasswordCreator>();
            mockHttpContext = new Mock<HttpContext>();

            indexService = new IndexService(mockCourseTutorRepository.Object, mockCourseStudentRepository.Object, mockPasswordCreator.Object);
        }

        public sealed class Constructor : IndexServiceTests
        {
            [Fact]
            public void GivenANullCourseTutorRepository_WhenConstructing_ThenThrowArgumentNullException()
            {
                FluentActions.Invoking(() => new IndexService(null, mockCourseStudentRepository.Object, mockPasswordCreator.Object))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseTutorRepository");
            }

            [Fact]
            public void GivenANullCourseStudentRepository_WhenConstructing_ThenThrowArgumentNullException()
            {
                FluentActions.Invoking(() => new IndexService(mockCourseTutorRepository.Object, null, mockPasswordCreator.Object))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("courseStudentRepository");
            }

            [Fact]
            public void GivenANullPasswordCreator_WhenConstructing_ThenThrowArgumentNullException()
            {
                FluentActions.Invoking(() => new IndexService(mockCourseTutorRepository.Object, mockCourseStudentRepository.Object, null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("passwordCreator");
            }
        }

        public sealed class GetTutorCourses : IndexServiceTests
        {
            [Fact]
            public void GivenANullAccountClaims_GetTutorCourses_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => indexService.GetTutorCourses(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("accountClaims");
            }
        }

        public sealed class GetStudentCourses : IndexServiceTests
        {
            [Fact]
            public void GivenANullAccountClaims_GetStudentCourses_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => indexService.GetStudentCourses(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("accountClaims");
            }
        }
        public sealed class ConstructDefaultInitialPassword : IndexServiceTests
        {
            [Fact]
            public void GivenANullAccountClaims_ConstructDefaultInitialPassword_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => indexService.ConstructDefaultInitialPassword(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("accountClaims");
            }

            [Fact]
            public void GivenAValidAccountClaims_ConstructDefaultInitialPassword_ShouldCallPasswordCreatorGenerateHashedPassword_Once()
            {
                var mockHttpContext = new Mock<HttpContext>();
                byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
                string saltString = Convert.ToBase64String(salt);

                var claims = new List<Claim>
                {
                    new Claim("FirstName", "John"),
                    new Claim("LastName", "Smith"),
                    new Claim("DateOfBirth", "140195"),
                    new Claim("Salt", saltString)
                };
                var identity = new ClaimsIdentity(claims);
                mockHttpContext.Setup(x => x.User.Claims).Returns(claims);

                var userClaims = mockHttpContext.Object.User.Claims;

                indexService.ConstructDefaultInitialPassword(new AccountClaims(userClaims.ToList()));
                mockPasswordCreator.Verify(x => x.GenerateHashedPassword(salt, "Js140195."), Times.Once());
            }
        }
    }
}
