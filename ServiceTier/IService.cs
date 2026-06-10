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
        Task DeleteByIdAsync(int Id); 
        Task<Boolean> ExistsByIdAsync(int Id);
    }
}
