using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly GymManagementDbContext _context;

        public Repository(GymManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.AddAsync(entity); 
        }

        public async Task DeleteByIdAsync(int Id)
        {
            var entity = await FindByIdAsync(Id);
            if(entity != null)
                _context.Remove(entity); 
        }

        public async Task<T?> FindByIdAsync(int Id)
        {
            return await _context.FindAsync<T>(Id);
        }

        public async Task<T?> GetByIdAsync
            (int Id, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>()
                .Where(e => EF.Property<int>(e, "Id") == Id);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            var query = _context.Set<T>(); 
            return await query.ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
           return await _context.SaveChangesAsync();
        }

        public async Task<Boolean> ExistsByIdAsync(int Id)
        {
            return await _context.Set<T>()
                .AnyAsync(e=>EF.Property<int>(e,"Id") == Id);
        }
    }
}
