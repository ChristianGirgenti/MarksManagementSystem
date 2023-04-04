using FluentAssertions;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories.Interfaces;
using MarksManagementSystem.Services.Classes;
using MarksManagementSystem.Helpers;
using Moq;
using MarksManagementSystem.Data.Repositories.Classes;
using MarksManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace MarksManagementSystem.Tests.Services
{
    public class AddTutorServiceTests
    {
        public readonly Mock<ITutorRepository> mockTutorRepository;
        public readonly Mock<IPasswordCreator> mockPasswordCreator;

        public readonly AddTutorService addTutorService;

        public AddTutorServiceTests()
        {
            mockTutorRepository = new Mock<ITutorRepository>();
            mockPasswordCreator = new Mock<IPasswordCreator>();

            addTutorService = new AddTutorService(mockTutorRepository.Object, mockPasswordCreator.Object);
        }

        public sealed class Constructor : AddTutorServiceTests
        {
            [Fact]
            public void GivenANullTutorRepository_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new AddTutorService(null, mockPasswordCreator.Object))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("tutorRepository");
            }

            [Fact]
            public void GivenANullPasswordCreator_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new AddTutorService(mockTutorRepository.Object, null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("passwordCreator");
            }
        }

        public sealed class AddTutor : AddTutorServiceTests
        {
            [Fact]
            public void GivenANullNewTutor_AddTutor_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => addTutorService.AddTutor(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("newTutor");
            }

            [Fact]
            public void GivenAValidNewTutor_AddTutor_ShouldCallTutorRepositoryAdd_ExactlyOnce()
            {
                var validTutor = new Tutor()
                {
                    TutorId = 1,
                    TutorFirstName = "Christian",
                    TutorLastName = "Girgenti",
                    TutorEmail = "christian.girgenti@myUniversity.co.uk",
                    TutorDateOfBirth = new DateTime(1995, 01, 14)
                };

                addTutorService.AddTutor(validTutor);
                mockTutorRepository.Verify(x => x.Add(validTutor), Times.Once);
            }
        }

        public sealed class SetNewTutorValue : AddTutorServiceTests
        {
            [Fact]
            public void GivenANullNewTutor_SetNewTutorValues_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => addTutorService.SetNewTutorValues(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("newTutor");
            }

            [Fact]
            public void GivenAValidNewTutor_SetNewTutorValues_ShouldCallGenerateHashedPassword_ExactlyOnce()
            {
                var validTutor = new Tutor()
                {
                    TutorId = 1,
                    TutorFirstName = "Christian",
                    TutorLastName = "Girgenti",
                    TutorEmail = "christian.girgenti@myUniversity.co.uk",
                    TutorDateOfBirth = new DateTime(1995, 01, 14)
                };
                var password = "Cg140195.";

                addTutorService.AddTutor(validTutor);
                mockPasswordCreator.Verify(x => x.GenerateHashedPassword(It.IsAny<byte[]>(), password), Times.Once);
            }
        }
    }
}
