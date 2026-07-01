using Microsoft.EntityFrameworkCore;
using RepositoryTier.Coach.DTOs;
using RepositoryTier.Coach.Enums;
using RepositoryTier.Coach.Repositories;
using RepositoryTier.Entities;
using RepositoryTier.Member.Enums;
using RepositoryTier.User;
using RepositoryTier.User.DTOs;
using RepositoryTier.User.Enums;
using RepositoryTier.User.Repositories;
using RepositoryTier.User.Results;
using ServiceTier.User;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier.Coach
{
    public class CoachService : ICoachService
    {
        private readonly ICoachRepository _repo; 
        private readonly IUserService _userService;
        public CoachService(
            ICoachRepository repo,
            IUserService userService)  
        { 
            _repo = repo; 
            _userService = userService;
        } 
        public async Task<GetCoachesResponse> GetCoachesAsync(GetCoachesRequest request)
        {  
            return await _repo.GetCoachesAsync(request);
        }

        public async Task<GetCoachByIdResponse?> GetByIdAsync(int Id)
        {
            return await _repo.GetByIdAsync(Id); 
        }

        public async Task<enUpdateCoachStatus> UpdateAsync(int Id,UpdateCoachByIdRequest request)
        {
            
            //1.Unique Email & Phone
            bool isUniqueEmail = await _userService.IsUniqueEmailAsync(request.Email, Id);
            bool isUniquePhone = await _userService.IsUniquePhoneAsync(request.Phone, Id);

            if (!isUniqueEmail)
                return enUpdateCoachStatus.NotUniqueEmail;

            if (!isUniquePhone)
                return enUpdateCoachStatus.NotUniquePhone;

            //2.Load then Update strategy for efCore Tracking
            var coach = await _repo.FindAsync(Id);
            if (coach == null)
                return enUpdateCoachStatus.CoachNotFound;
            coach.HireDate = request.HireDate;
            coach.IsActive = request.IsActive;
            coach.Phone = request.Phone.Trim();
            coach.Email = request.Email.Trim();
            coach.FullName = request.FullName.Trim();
            coach.Salary = request.Salary;
            coach.Specialization = request.Specialization; 

            EntityState state = _repo.GetEntityState(coach);
            if (state == EntityState.Unchanged)
                return enUpdateCoachStatus.DataNotChanged;
            coach.UpdatedAt = DateTime.UtcNow;

            //3.Save
            int affectedRows = await _repo.SaveChangesAsync();
            return enUpdateCoachStatus.Succeeded;
        }

        public async Task<List<CoachLookUpResponse>> GetLookUpCoachesAsync()
        {
            return await _repo.GetLookUpCoachesAsync();
        }
    }
}
