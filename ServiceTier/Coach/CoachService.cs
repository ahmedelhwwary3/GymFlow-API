using RepositoryTier.Coach.DTOs;
using RepositoryTier.Coach.Enums;
using RepositoryTier.Coach.Repositories;
using RepositoryTier.Entities;
using RepositoryTier.User.Enums;
using ServiceTier.User;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace ServiceTier.Coach
{
    public class CoachService : Service<RepositoryTier.Entities.Coach>, ICoachService
    {
        private readonly ICoachRepository _repo;
        private readonly IUserService _userService;
        public CoachService(
            ICoachRepository repo,
            IUserService userService) 
            : base(repo) 
        { 
            _repo = repo;
            _userService = userService;
        }
         
        private async Task<Boolean> IsUniquePhone(int userId,string phone)
        {
            var user = await _userService.FindByIdAsync(userId);
            if (user == null)
                return false; 
            //Same user
            if(user.Phone == phone)
                return true;

            return !await _userService
                .ExistsByPhoneAsync(phone);
        }

        private async Task<Boolean> IsUniqueEmail(int userId, string email)
        {
            var user = await _userService.FindByIdAsync(userId);
            if (user == null)
                return false;

            //Same user
            if (user.Email == email)
                return true;

            return !await _userService
                .ExistsByEmailAsync(email);
        }

        public async Task<AddCoachResponse> AddAsync(AddCoachRequest request)
        {
            var newCoach = new RepositoryTier.Entities.Coach()
            {
                CreatedAt=DateTime.UtcNow,
                DateOfBirth=request.DateOfBirth,
                Email=request.Email.Trim(),
                FullName=request.FullName.Trim(),
                Gender=request.Gender,
                HireDate= DateOnly.FromDateTime(DateTime.UtcNow),
                PasswordHash=BCrypt.Net.BCrypt.HashPassword(request.Password),
                Phone=request.Phone.Trim(),
                Role=enUserRole.Coach,
                Salary=request.Salary,
                Specialization=request.Specialization 
            };

            await _repo.AddAsync(newCoach);
            int affectedRows= await _repo.SaveChangesAsync();

            return new AddCoachResponse()
            {
                HireDate=newCoach.HireDate,
                Salary=newCoach.Salary,
                Id=newCoach.Id,
                Specialization=newCoach.Specialization
            };

        }

        public async Task<GetCoachesResponse> GetCoachesAsync(GetCoachesRequest request)
        {  
            return await _repo.GetCoachesAsync(request);
        }

        public async Task<GetCoachByIdResponse?> GetByIdAsync(int Id)
        {
            return await _repo.GetCoachByIdAsync(Id); 
        }

        public async Task<enUpdateCoachByIdStatus> UpdateByIdAsync(int Id,UpdateCoachByIdRequest request)
        {
            var coach = await _repo.FindByIdAsync(Id);
            if (coach == null)
                return enUpdateCoachByIdStatus.CoachNotFound;
              
            bool isUniqueEmail = await IsUniqueEmail(coach.Id, request.Email);
            bool isUniquePhone = await IsUniquePhone(coach.Id, request.Phone);

            if (!isUniqueEmail)
                return enUpdateCoachByIdStatus.NotUniqueEmail;

            if (!isUniqueEmail)
                return enUpdateCoachByIdStatus.NotUniquePhone;

            coach.HireDate = request.HireDate;
            coach.IsActive = request.IsActive;
            coach.Phone = request.Phone;
            coach.Email = request.Email;
            coach.FullName = request.FullName;
            coach.Salary = request.Salary;
            coach.Specialization = request.Specialization;

            int affectedRows = await _repo.SaveChangesAsync();
            return affectedRows>0? enUpdateCoachByIdStatus.Succeeded:
                enUpdateCoachByIdStatus.DataNotChanged;
        }
    }
}
