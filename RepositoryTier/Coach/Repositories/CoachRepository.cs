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
        protected readonly PaganationOptions _paganationOptions;
        public CoachRepository(
            GymManagementDbContext context
            , IOptions<PaganationOptions> configs) : base(context) 
        {
            _paganationOptions = configs.Value;
            if (_paganationOptions == null)
                throw new Exception("JWT options is not configured");
        }
         
        public async Task<GetCoachesResponse> GetCoachesAsync(GetCoachesRequest request)
        {
            string? search = request.Search?.Trim();
            int page = request.Page ?? (Convert.ToInt32(_paganationOptions.Page));
            int pageSize = request.PageSize ?? (Convert.ToInt32(_paganationOptions.TinyPageSize));

            var query = _context.Coaches
                .AsNoTracking() // Read-Only (Better C# performance)
                .Where(c =>(request.Specialization == null ||
                c.Specialization == request.Specialization) &&
            (request.Status == enUserActivityStatus.All ||
            (c.IsActive == (request.Status == enUserActivityStatus.Active ? true : false))) &&
            (string.IsNullOrEmpty(search) ||
            (c.FullName.Contains(search) || c.Email.Contains(search) || c.Phone.Contains(search))))
                .Select(c => new CoachResponse()
                {
                    AssignedMembersCount = c.Members.Count(),
                    Email = c.Email,
                    FullName = c.FullName,
                    Id = c.Id,
                    Phone = c.Phone,
                    Specialization = c.Specialization
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
            var response = new GetCoachesResponse()
            {
                Coaches=coaches,
                Specializations=Enum.GetValues<enCoachSpecialization>().ToList(),
                Statuses=Enum.GetValues<enUserActivityStatus>().ToList(),
            };

            return response;
            
        }
    }
}
