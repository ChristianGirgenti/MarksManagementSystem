using FluentAssertions;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories.Interfaces;
using MarksManagementSystem.Services.Classes;
using MarksManagementSystem.Helpers;
using Moq;

namespace MarksManagementSystem.Tests.Services
{
    public class AddStudentServiceTests
    {
        public readonly Mock<IStudentRepository> mockStudentRepository;
        public readonly Mock<IPasswordCreator> mockPasswordCreator;

        public readonly AddStudentService addStudentService;

        public AddStudentServiceTests()
        {
            mockStudentRepository = new Mock<IStudentRepository>();
            mockPasswordCreator = new Mock<IPasswordCreator>();

            addStudentService = new AddStudentService(mockStudentRepository.Object, mockPasswordCreator.Object);
        }

        public sealed class Constructor : AddStudentServiceTests
        {
            [Fact]
            public void GivenANullStudentRepository_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new AddStudentService(null, mockPasswordCreator.Object))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("studentRepository");
            }

            [Fact]
            public void GivenANullPasswordCreator_WhenConstructing_ThenThrowArgumentNullException()
            {

                FluentActions.Invoking(() => new AddStudentService(mockStudentRepository.Object, null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("passwordCreator");
            }
        }

        public sealed class AddStudent : AddStudentServiceTests
        {
            [Fact]
            public void GivenANullNewStudent_AddStudent_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => addStudentService.AddStudent(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("newStudent");
            }

            [Fact]
            public void GivenAValidNewStudent_AddStudent_ShouldCallStudentRepositoryAdd_ExactlyOnce()
            {
                var validStudent = new Student()
                {
                    StudentId = 1,
                    StudentFirstName = "Christian",
                    StudentLastName = "Girgenti",
                    StudentEmail = "christian.girgenti@myUniversity.co.uk",
                    StudentDateOfBirth = new DateTime(1995, 01, 14)
                };

                addStudentService.AddStudent(validStudent);
                mockStudentRepository.Verify(x => x.Add(validStudent), Times.Once);
            }
        }

        public sealed class SetNewStudentValue : AddStudentServiceTests
        {
            [Fact]
            public void GivenANullNewStudent_SetNewStudentValues_ShouldThrowArgumentNullException()
            {
                FluentActions.Invoking(() => addStudentService.SetNewStudentValues(null))
                    .Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithParameterName("newStudent");
            }

            [Fact]
            public void GivenAValidNewStudent_SetNewStudentValues_ShouldCallGenerateHashedPassword_ExactlyOnce()
            {
                var validStudent = new Student()
                {
                    StudentId = 1,
                    StudentFirstName = "Christian",
                    StudentLastName = "Girgenti",
                    StudentEmail = "christian.girgenti@myUniversity.co.uk",
                    StudentDateOfBirth = new DateTime(1995, 01, 14)
                };
                var password = "Cg140195.";

                addStudentService.AddStudent(validStudent);
                mockPasswordCreator.Verify(x => x.GenerateHashedPassword(It.IsAny<byte[]>(), password), Times.Once);
            }
        }
    }
}
