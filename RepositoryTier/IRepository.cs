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
        Task<T?> FindByIdAsync(int Id);
        Task DeleteByIdAsync(int Id);
        Task AddAsync(T entity);
        Task<Boolean> ExistsByIdAsync(int Id);
        Task<int> SaveChangesAsync();
        Task<List<T>> GetAllAsync();
    }
}
