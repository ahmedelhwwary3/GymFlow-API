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
        Task<T?> FindByIdAsync(int Id);
        Task DeleteByIdAsync(int Id);
        Task AddAsync(T entity);
        Task<Boolean> ExistsByIdAsync(int Id);
    }
}
