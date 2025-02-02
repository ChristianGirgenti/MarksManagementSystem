﻿using MarksManagementSystem.Data.Models;

namespace MarksManagementSystem.Data.Repositories.Interfaces
{
    public interface ITutorRepository
    {
        public void Add(Tutor tutor);
        public void Update(Tutor tutor);
        public Tutor GetById(int tutorId);
        public void Delete(int tutorId);
        public List<Tutor> GetAll();
        public void UpdatePasswordByTutorId(int tutorId, string newPassword);
    }
}
