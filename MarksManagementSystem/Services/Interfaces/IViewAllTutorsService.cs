using MarksManagementSystem.ViewModel;
using System.Security.Claims;

namespace MarksManagementSystem.Services.Interfaces
{
    public interface IViewAllTutorsService
    {
        public List<ViewAllTutorsViewModel> GetAllTutorsViewModel();
        public bool IsTutorUnitLeader(int tutorId);
        public bool DeleteTutor(int tutorId, IEnumerable<Claim> claims);
    }
}
