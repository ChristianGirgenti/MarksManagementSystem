using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Helpers;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using MarksManagementSystem.Services.Interfaces;
using MarksManagementSystem.Data.Repositories.Interfaces;

namespace MarksManagementSystem.Services.Classes
{
    public class EditCourseService : IEditCourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ITutorRepository _tutorRepository;
        private readonly ICourseTutorRepository _courseTutorRepository;

        public EditCourseService(ICourseRepository courseRepository, ITutorRepository tutorRepository, ICourseTutorRepository courseTutorRepository)
        {
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            _tutorRepository = tutorRepository ?? throw new ArgumentNullException(nameof(tutorRepository));
            _courseTutorRepository = courseTutorRepository ?? throw new ArgumentNullException(nameof(courseTutorRepository));
        }

        public Course GetCourseToEditById(int courseId)
        {
            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            return _courseRepository.GetById(courseId);
        }

        public List<CourseTutor> GetAllTheCourseTutors(int courseId)
        {
            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            return _courseTutorRepository.GetAll().Where(ct => ct.CourseId == courseId).ToList();
        }

        public int GetUnitLeaderId(List<CourseTutor> allCourseTutors, int courseId)
        {
            if (allCourseTutors == null) throw new ArgumentNullException(nameof(allCourseTutors));
            return allCourseTutors.First(ct => ct.CourseId == courseId && ct.IsUnitLeader).TutorId;
        }

        public List<SelectListItem> ShowPossibleUnitLeaderInSelectionList(int unitLeaderId)
        {
            if (unitLeaderId <= 0) throw new ArgumentOutOfRangeException(nameof(unitLeaderId));

            List<Tutor> unitLeaders = _courseTutorRepository.GetAll()
                .Where(ct => ct.IsUnitLeader)
                .Select(ct => ct.Tutor)
                .ToList();

            List<SelectListItem> nonUnitLeaders = _tutorRepository.GetAll()
                .Where(t => !unitLeaders.Contains(t))
                .Select(t => new SelectListItem { Value = t.TutorId.ToString(), Text = t.TutorFirstName + " " + t.TutorLastName })
                .ToList();

            var possibleUnitLeaderTutors = nonUnitLeaders;

            var currentUnitLeader = unitLeaders.FirstOrDefault(uL => uL.TutorId == unitLeaderId);

            if (currentUnitLeader != null)
                possibleUnitLeaderTutors.Insert(0, new SelectListItem { Value = currentUnitLeader.TutorId.ToString(), Text = currentUnitLeader.TutorFirstName + " " + currentUnitLeader.TutorLastName });

            return possibleUnitLeaderTutors;
        }

        public List<SelectListItem> PopulateOtherTutors(int unitLeaderId, int courseId)
        {
            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            if (unitLeaderId <= 0) throw new ArgumentOutOfRangeException(nameof(unitLeaderId));

            var currentOtherTutors = _courseTutorRepository.GetAll()
                .Where(ct => ct.CourseId == courseId && ct.IsUnitLeader == false).Select(t => t.Tutor).ToList();

            List<SelectListItem> otherTutors = _tutorRepository.GetAll()
                .Where(x => x.TutorId != unitLeaderId)
                .Select(t => new SelectListItem
                {
                    Value = t.TutorId.ToString(),
                    Text = t.TutorFirstName + " " + t.TutorLastName,
                    Selected = currentOtherTutors.Contains(t)
                })
                .ToList();

            return otherTutors;
        }

        public AddEditCourseViewModel FormatNewCourseValues(AddEditCourseViewModel editCourseViewModel)
        {
            if (editCourseViewModel == null) throw new ArgumentNullException(nameof(editCourseViewModel));
            editCourseViewModel.CourseName = StringUtilities.Capitalise(editCourseViewModel.CourseName);
            return editCourseViewModel;
        }

        public Course EditCourse(int courseId, AddEditCourseViewModel editCourseViewModel)
        {
            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            if (editCourseViewModel == null) throw new ArgumentNullException(nameof(editCourseViewModel));

            var courseEdited = new Course
            {
                CourseId = courseId,
                CourseName = editCourseViewModel.CourseName,
                CourseCredits = editCourseViewModel.CourseCredits,
            };


            _courseRepository.Update(courseEdited);
            return courseEdited;
        }

        public void ChangeTutorCourseRelationships(AddEditCourseViewModel editCourseViewModel, Course courseEdited, List<string> tutorIds)
        {
            if (editCourseViewModel == null) throw new ArgumentNullException(nameof(editCourseViewModel));
            if (courseEdited == null) throw new ArgumentNullException(nameof(courseEdited));

            _courseTutorRepository.DeleteCourseUnitLeaderRelationshipByCourseId(courseEdited.CourseId);
            _courseTutorRepository.DeleteAllOtherTutorsInACourse(courseEdited.CourseId);

            var newUnitLeader = _tutorRepository.GetAll().SingleOrDefault(t => t.TutorId == editCourseViewModel.UnitLeaderId);
            if (newUnitLeader != null)
                AddUnitLeaderRelationship(newUnitLeader, courseEdited);

            editCourseViewModel.TutorIds = tutorIds;

            AddOtherTutorsRelationship(editCourseViewModel.TutorIds, courseEdited, editCourseViewModel);
        }

        public void AddOtherTutorsRelationship(List<string> otherTutors, Course courseEdited, AddEditCourseViewModel editCourseViewModel)
        {
            if (otherTutors == null) throw new ArgumentNullException(nameof(otherTutors));
            if (courseEdited == null) throw new ArgumentNullException(nameof(courseEdited));
            if (editCourseViewModel == null) throw new ArgumentNullException(nameof(editCourseViewModel));

            //If a tutor is selected to be both unitLeader and otherTutor, unit leader will have priority, ignoring the other tutor selection
            foreach (var tutorId in otherTutors)
            {
                var tutor = _tutorRepository.GetById(Convert.ToInt32(tutorId));
                if (tutor.TutorId != editCourseViewModel.UnitLeaderId)
                {
                    CourseTutor courseTutor = new()
                    {
                        CourseId = courseEdited.CourseId,
                        TutorId = tutor.TutorId,
                        Course = courseEdited,
                        Tutor = tutor,
                        IsUnitLeader = false
                    };
                    _courseTutorRepository.Add(courseTutor);
                }
            }
        }

        public void AddUnitLeaderRelationship(Tutor newUnitLeader, Course courseEdited)
        {
            if (newUnitLeader == null) throw new ArgumentNullException(nameof(newUnitLeader));
            if (courseEdited == null) throw new ArgumentNullException(nameof(courseEdited));

            var newCourseTutorRelationship = new CourseTutor()
            {
                CourseId = courseEdited.CourseId,
                TutorId = newUnitLeader.TutorId,
                Course = courseEdited,
                Tutor = newUnitLeader,
                IsUnitLeader = true
            };

            _courseTutorRepository.Add(newCourseTutorRelationship);           
        }
    }
}
