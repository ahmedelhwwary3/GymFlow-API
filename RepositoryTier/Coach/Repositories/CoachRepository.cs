using RepositoryTier.Coach.DTOs;
using RepositoryTier.User.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryTier.Coach.Enums; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RepositoryTier.API_Configurations;

namespace RepositoryTier.Coach.Repositories
{
    public class CoachRepository : Repository<Entities.Coach>, ICoachRepository
    {
        protected readonly PaganationOptions PaganationOptions;
    
        public CoachRepository(
            GymManagementDbContext context
            , IOptions<PaganationOptions> configs) : base(context) 
        {
            PaganationOptions = configs.Value;
            if (PaganationOptions == null)
                throw new Exception("Paganation options is not configured");
        }
         
        public async Task<GetCoachesResponse> GetCoachesAsync(GetCoachesRequest request)
        {
            string? search = request.Search?.Trim();
            int page = request.Page ?? PaganationOptions.Page;
            int pageSize = request.PageSize ?? PaganationOptions.TinyPageSize;

            var query = _context.Coaches
                .AsNoTracking() // Read-Only (Better C# performance)
                .Where(c => (request.Specialization == null ||
                c.Specialization == request.Specialization) &&
            (request.IsActive == null || (c.IsActive == request.IsActive)) &&
            (string.IsNullOrEmpty(search) ||
            (c.FullName.Contains(search) || c.Email.Contains(search) || c.Phone.Contains(search))))
                .Select(c => new CoachResponse()
                {
                    AssignedMembersCount = c.Members.Count(),
                    Email = c.Email,
                    FullName = c.FullName,
                    Id = c.Id,
                    Phone = c.Phone,
                    Specialization = c.Specialization,
                    IsActive = c.IsActive
                });

            query = request.Sort switch
            {
                enCoachSort.FullName => query.OrderBy(c => c.FullName),
                enCoachSort.Email => query.OrderBy(c => c.Email),
                _ => query.OrderBy(c => c.AssignedMembersCount)
            };

            query = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var coaches = await query.ToListAsync();
            int count = await _context.Coaches.CountAsync();

            var response = new GetCoachesResponse()
            {
                Coaches = coaches,
                Specializations = Enum.GetValues<enCoachSpecialization>().ToList(),
                Count = count
            };

            return response;
            
        }

        public async Task<GetCoachByIdResponse?> GetByIdAsync(int Id)
        {
            return await _context.Coaches
                .AsNoTracking()
                .Where(c => c.Id == Id)
                .Select(c => new GetCoachByIdResponse()
                {
                    Id= c.Id,
                    Email = c.Email,
                    FullName = c.FullName,
                    Gender = c.Gender,
                    HireDate = c.HireDate,
                    Phone = c.Phone,
                    Salary = c.Salary,
                    Specialization = c.Specialization,
                    IsActive = c.IsActive 
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> IsActiveByIdAsync(int Id)
        {
            return await _context.Coaches
                .AnyAsync(c => c.Id == Id && c.IsActive);
        }

        public async Task<List<CoachLookUpResponse>> GetLookUpCoachesAsync()
        {
            return await _context.Coaches
                .Select(c => new CoachLookUpResponse()
                {
                    FullName = c.FullName,
                    Id = c.Id
                }).ToListAsync();
        }
    }
}
