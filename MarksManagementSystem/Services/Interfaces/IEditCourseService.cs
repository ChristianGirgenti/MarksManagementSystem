﻿using MarksManagementSystem.Data.Models;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarksManagementSystem.Services.Interfaces
{
    public interface IEditCourseService
    {
        public Course GetCourseToEditById(int courseId);
        public List<CourseTutor> GetAllTheCourseTutors(int courseId);
        public int GetUnitLeaderId(List<CourseTutor> allCourseTutors, int courseId);
        public List<SelectListItem> ShowPossibleUnitLeadersInSelectionList(int unitLeaderId);
        public List<SelectListItem> PopulateOtherTutors(int unitLeaderId, int courseId);
        public AddEditCourseViewModel FormatNewCourseValues(AddEditCourseViewModel editCourseViewModel);
        public void ChangeCourseTutorRelationships(AddEditCourseViewModel editCourseViewModel, Course courseEdited, List<string> tutorIds);
        public Course EditCourse(int courseId, AddEditCourseViewModel editCourseViewModel);
        public void AddOtherTutorsRelationship(List<string> otherTutors, Course courseEdited, AddEditCourseViewModel editCourseViewModel);
        public void AddUnitLeaderRelationship(Tutor newUnitLeader, Course courseEdited);


    }
}
