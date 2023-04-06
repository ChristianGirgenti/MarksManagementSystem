using FluentAssertions;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Services.Classes;
using MarksManagementSystem.Helpers;
using Moq;
using MarksManagementSystem.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MarksManagementSystem.Tests.Services
{
    public class LoginServiceTests
    {
        public readonly Mock<IPasswordCreator> mockPasswordCreator;
        public readonly Mock<HttpContext> mockHttpContext;
        public readonly MarksManagementContext marksManagementContext;
        public readonly LoginService loginService;

        public LoginServiceTests()
        {
            var options = new DbContextOptionsBuilder<MarksManagementContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            mockHttpContext = new Mock<HttpContext>();
            marksManagementContext = new MarksManagementContext(options);
            mockPasswordCreator = new Mock<IPasswordCreator>();
            loginService = new LoginService(marksManagementContext, mockPasswordCreator.Object);
        }

        public sealed class Constructor : LoginServiceTests
        {
            [Fact]
            public void GivenANullMarksManagementContext_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new LoginService(null, mockPasswordCreator.Object))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("context");
            }

            [Fact]
            public void GivenANullPasswordCreator_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new LoginService(marksManagementContext, null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("passwordCreator");
            }
        }

        public sealed class LogInTutorIsSuccess : LoginServiceTests
        {
            [Fact]
            public void GivenANullCredential_LogInTutorIsSuccess_ThrowsArgumentNullException()
            {
                FluentActions.Invoking(() => loginService.LogInTutorIsSuccess(null, mockHttpContext.Object))
                    .Should()
                    .ThrowExactlyAsync<ArgumentNullException>()
                    .WithParameterName("credential");
            }

            [Fact]
            public void GivenANullHttpContext_LogInTutorIsSuccess_ThrowsArgumentNullException()
            {
                FluentActions.Invoking(() => loginService.LogInTutorIsSuccess(new Credential(), null))
                    .Should()
                    .ThrowExactlyAsync<ArgumentNullException>()
                    .WithParameterName("httpContext");
            }         
        }

        public sealed class LogInStudentIsSuccess : LoginServiceTests
        {
            [Fact]
            public void GivenANullCredential_LogInStudentIsSuccess_ThrowsArgumentNullException()
            {
                FluentActions.Invoking(() => loginService.LogInStudentIsSuccess(null, mockHttpContext.Object))
                    .Should()
                    .ThrowExactlyAsync<ArgumentNullException>()
                    .WithParameterName("credential");
            }

            [Fact]
            public void GivenANullHttpContext_LogInStudentIsSuccess_ThrowsArgumentNullException()
            {
                FluentActions.Invoking(() => loginService.LogInStudentIsSuccess(new Credential(), null))
                    .Should()
                    .ThrowExactlyAsync<ArgumentNullException>()
                    .WithParameterName("httpContext");
            }
        }

        public sealed class BuildClaimsTutor : LoginServiceTests
        {
            [Fact]
            public void GivenANullTutor_BuildClaimsTutor_ThrowsArgumentNullException()
            {
                FluentActions.Invoking(() => loginService.BuildClaimsTutor(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("tutor");
            }
        }

        public sealed class BuildClaimsStudent : LoginServiceTests
        {
            [Fact]
            public void GivenANullStudent_BuildClaimsStudent_ThrowsArgumentNullException()
            {
                FluentActions.Invoking(() => loginService.BuildClaimsStudent(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("student");
            }
        }
    }
}
