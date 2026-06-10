using RepositoryTier.Coach.DTOs;
using RepositoryTier.Coach.Repositories; 
using RepositoryTier.User.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace ServiceTier.Coach
{
    public class CoachService : Service<RepositoryTier.Entities.Coach>, ICoachService
    {
        protected readonly ICoachRepository _repo;
        public CoachService(ICoachRepository repo) : base(repo)
        {
            _repo=repo;
        }

        public async Task<AddCoachResponse> AddAsync(AddCoachRequest request)
        {
            var newCoach = new RepositoryTier.Entities.Coach()
            {
                CreatedAt=DateTime.UtcNow,
                DateOfBirth=request.DateOfBirth,
                Email=request.Email,
                FullName=request.FullName,
                Gender=request.Gender,
                HireDate= DateOnly.FromDateTime(DateTime.UtcNow),
                PasswordHash=BCrypt.Net.BCrypt.HashPassword(request.Password),
                Phone=request.Phone,
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
    }
}
