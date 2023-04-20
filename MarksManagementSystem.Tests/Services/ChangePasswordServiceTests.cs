using FluentAssertions;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories.Interfaces;
using MarksManagementSystem.Helpers;
using MarksManagementSystem.Services.Classes;
using Microsoft.AspNetCore.Http;
using Moq;


namespace MarksManagementSystem.Tests.Services
{
    public class ChangePasswordServiceTests
    {
        public readonly Mock<IPasswordCreator> mockPasswordCreator;
        public readonly Mock<ITutorRepository> mockTutorRepository;
        public readonly Mock<IStudentRepository> mockStudentRepository;
        public readonly Mock<HttpContext> mockHttpContext;

        public readonly ChangePasswordService changePasswordService;

        public ChangePasswordServiceTests()
        {
            mockPasswordCreator = new Mock<IPasswordCreator>();
            mockTutorRepository = new Mock<ITutorRepository>();
            mockStudentRepository = new Mock<IStudentRepository>();
            mockHttpContext = new Mock<HttpContext>();

            changePasswordService = new ChangePasswordService(mockPasswordCreator.Object, mockTutorRepository.Object, mockStudentRepository.Object);
        }

        public sealed class Constructor : ChangePasswordServiceTests
        {
            [Fact]
            public void GivenANullPasswordCreator_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new ChangePasswordService(null, mockTutorRepository.Object, mockStudentRepository.Object))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("passwordCreator");
            }

            [Fact]
            public void GivenANullTutorRepository_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new ChangePasswordService(mockPasswordCreator.Object, null, mockStudentRepository.Object))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("tutorRepository");
            }

            [Fact]
            public void GivenANullStudentRepository_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new ChangePasswordService(mockPasswordCreator.Object, mockTutorRepository.Object, null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("studentRepository");
            }
        }

        public sealed class ChangePassword : ChangePasswordServiceTests
        {
            [Fact]
            public void GivenANullContext_WhenChangePassword_ThrowArgumentNullException()
            {

                FluentActions.Invoking(() => changePasswordService.ChangePassword(null, "currentPassword", "newPassword"))
                    .Should()
                    .ThrowExactlyAsync<ArgumentNullException>()
                    .WithParameterName("context");
            }

            [Theory]
            [InlineData(" ")]
            [InlineData(null)]
            public void GivenANullOrEmptyCurrentPassword_WhenChangePassword_ThrowArgumentNullException(string currentPassword)
            {
                var mockHttpContext = new Mock<HttpContext>();
                FluentActions.Invoking(() => changePasswordService.ChangePassword(mockHttpContext.Object, currentPassword, "newPassword"))
                    .Should()
                    .ThrowExactlyAsync<ArgumentNullException>()
                    .WithParameterName(nameof(currentPassword));            
            }

            [Theory]
            [InlineData(" ")]
            [InlineData(null)]
            public void GivenANullOrEmptyNewPassword_WhenChangePassword_ThrowArgumentNullException(string newPassword)
            {
                FluentActions.Invoking(() => changePasswordService.ChangePassword(mockHttpContext.Object, "currentPassword", newPassword))
                    .Should()
                    .ThrowExactlyAsync<ArgumentNullException>()
                    .WithParameterName(nameof(newPassword));
            }
        }
    }
}
