using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories.Interfaces;
using MarksManagementSystem.Services.Interfaces;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarksManagementSystem.Services.Classes
{
    public class ViewAllTutorsService : IViewAllTutorsService
    {
        private readonly ITutorRepository _tutorRepository;
        private readonly ICourseTutorRepository _courseTutorRepository;

        public ViewAllTutorsService(ITutorRepository tutorRepository, ICourseTutorRepository courseTutorRepository)
        {
            _tutorRepository = tutorRepository;
            _courseTutorRepository = courseTutorRepository;
        }

        public List<ViewAllTutorsViewModel> GetAllTutorsViewModel()
        {
            return _tutorRepository.GetAll()
               .Select(t => new ViewAllTutorsViewModel
               {
                   TutorId = t.TutorId,
                   TutorFullName = t.TutorFirstName + " " + t.TutorLastName,
                   TutorEmail = t.TutorEmail,
                   TutorDateOfBirth = t.TutorDateOfBirth.Date.ToString("d"),
                   CourseLed = _courseTutorRepository.GetAll()
                       .Where(ct => ct.TutorId == t.TutorId && ct.IsUnitLeader)
                       .Select(ct => ct.Course.CourseName)
                       .SingleOrDefault(),

                   OtherCourses = string.Join(", ", _courseTutorRepository.GetAll()
                       .Where(ct => ct.TutorId == t.TutorId && ct.IsUnitLeader == false)
                       .Select(ct => ct.Course.CourseName)
                       .ToList())

               })
               .ToList();
        }

        public bool IsTutorUnitLeader(int tutorId)
        {
            if (tutorId < 0) throw new ArgumentOutOfRangeException(nameof(tutorId));
            return _courseTutorRepository.GetAll().Where(ct => ct.TutorId == tutorId && ct.IsUnitLeader == true).Any();
        }

        public bool DeleteTutor(int tutorId, IEnumerable<Claim> claims)
        {
            if (tutorId < 0) throw new ArgumentOutOfRangeException(nameof(tutorId));
            if (_tutorRepository.GetById(tutorId).TutorEmail != (claims.FirstOrDefault(c => c.Type == "Email")?.Value))
            {
                _tutorRepository.Delete(tutorId);
                return true;
            }
            return false;
        }
    }
}
