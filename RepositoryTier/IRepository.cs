using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier
{
    public interface IRepository<T> where T : class
    { 
        Task<T?> GetByIdAsync(int Id,params Expression<Func<T, object>>[]includes);
        Task<T?> FindAsync(int Id);
        Task DeleteAsync(int Id);
        Task AddAsync(T entity);
        Task<Boolean> ExistsAsync(int Id);
        Task<int> SaveChangesAsync();
        Task<List<T>> GetAllAsync();
        EntityState GetEntityState(T entity);
    }
}
