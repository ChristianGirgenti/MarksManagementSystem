using MarksManagementSystem.Data.Models;

namespace MarksManagementSystem.Data.Repositories
{
    public interface ITutorRepository
    {
        public void Add(Tutor course);
        public void Update(Tutor course);
        public Tutor GetById(int id);
        public void Delete(int id);
        public List<Tutor> GetAll();
    }
}
