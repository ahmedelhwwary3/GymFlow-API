using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier
{
    public interface IService<T> where T : class
    { 
        Task DeleteAsync(int Id); 
        Task<Boolean> ExistsAsync(int Id);
        Task<T?> FindAsync(int Id);
        Task AddAsync(T entity);
    }
}
