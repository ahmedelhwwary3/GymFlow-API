using Microsoft.EntityFrameworkCore;
using RepositoryTier.User.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.User.Repositories
{
    public class UserRepository : Repository<Entities.User>, IUserRepository
    {
        public UserRepository(GymManagementDbContext context) : base(context) { }

        public async Task<Entities.User?> GetByEmailAsync(string email, bool isTracked)
        {
            var query = _context.Users.Where(u => u.Email == email);

            if (isTracked)
                return await query.SingleOrDefaultAsync();

            return await query.AsNoTracking()
                .SingleOrDefaultAsync();
        } 

        public async Task<Entities.User?> GetByPhoneAsync(string phone, bool isTracked)
        {
            var query = _context.Users.Where(u => u.Phone == phone);

            if (isTracked)
                return await query.SingleOrDefaultAsync();

            return await query.AsNoTracking()
                .SingleOrDefaultAsync();

        }
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsByPhoneAsync(string phone)
        {
            return await _context.Users.AnyAsync(u => u.Phone == phone);
        }

        public async Task<int?> GetIdByIdentifierAsync(string identifier)
        {
            return await _context.Users
                .Where(u => u.Email == identifier || u.Phone == identifier)
                .Select(u => u.Id)
                .SingleOrDefaultAsync();
        }

        public async Task<string?> GetPhoneByIdAsync(int Id)
        {
            return await _context.Users
                .Where(u => u.Id == Id)
                .Select(u => u.Phone)
                .SingleOrDefaultAsync();
        }

        public async Task<string?> GetEmailByIdAsync(int Id)
        {
            return await _context.Users
                .Where(u => u.Id == Id)
                .Select(u => u.Email)
                .SingleOrDefaultAsync();
        }

        public async Task<GetUserByIdResponse?> GetUserByIdAsync(int Id)
        {
            try
            {
                return await _context.Users
                .Where(u => u.Id == Id)
                .Select(u => new GetUserByIdResponse()
                {
                    IsActive = u.IsActive,
                    DateOfBirth = u.DateOfBirth,
                    Email = u.Email,
                    FullName = u.FullName,
                    Gender = u.Gender,
                    Id = Id,
                    Phone = u.Phone
                }).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
              
        }
    }
}
