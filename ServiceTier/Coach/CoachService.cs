using RepositoryTier.Coach.DTOs;
using RepositoryTier.Coach.Enums;
using RepositoryTier.Coach.Repositories;
using RepositoryTier.Coach.Results;
using RepositoryTier.Entities;
using RepositoryTier.User.Enums;
using ServiceTier.User;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryTier.User;
using RepositoryTier.User.Repositories;

namespace ServiceTier.Coach
{
    public class CoachService : Service<RepositoryTier.Entities.Coach>, ICoachService
    {
        private readonly ICoachRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly IUserService _userService;
        public CoachService(
            ICoachRepository repo,
            IUserRepository userRepo,
            IUserService userService) 
            : base(repo) 
        { 
            _repo = repo;
            _userRepo = userRepo;
            _userService = userService;
        }
           
        public async Task<AddCoachResult> AddAsync(AddCoachRequest request)
        {
            bool isUniqueEmail = await _userService.IsUniqueEmailAsync(request.Email);
            bool isUniquePhone = await _userService.IsUniquePhoneAsync(request.Phone);

            if (!isUniqueEmail)
                return new AddCoachResult(enAddCoachStatus.NotUniqueEmail);

            if (!isUniquePhone)
                return new AddCoachResult(enAddCoachStatus.NotUniqueEmail);

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

            if (affectedRows > 0)
            {
                var response = new AddCoachResponse()
                {
                    HireDate = newCoach.HireDate,
                    Salary = newCoach.Salary,
                    NewId = newCoach.Id,
                    Specialization = newCoach.Specialization
                };
                return new AddCoachResult(enAddCoachStatus.Succeeded,response);
            }

            return new AddCoachResult(enAddCoachStatus.InternalServerError);
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
              
            bool isUniqueEmail = await _userService.IsUniqueEmailAsync(request.Email, coach.Id);
            bool isUniquePhone = await _userService.IsUniquePhoneAsync(request.Phone,coach.Id);

            if (!isUniqueEmail)
                return enUpdateCoachByIdStatus.NotUniqueEmail;

            if (!isUniquePhone)
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

        public async Task<List<CoachLookUpResponse>> GetLookUpCoachesAsync()
        {
            return await _repo.GetLookUpCoachesAsync();
        }
    }
}
