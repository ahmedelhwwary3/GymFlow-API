using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations;
using RepositoryTier;

namespace ServiceTier
{
    public abstract class Service<T> : IService<T> where T : class
    {
        private readonly IRepository<T> _repo;
        public Service(IRepository<T>repo)
        {
            _repo = repo;
        }
        public async Task AddAsync(T entity)
        {
            await _repo.AddAsync(entity);
        }

        public async Task DeleteAsync(int Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<bool> ExistsAsync(int Id)
        {
            return await _repo.ExistsAsync(Id);
        }

        async Task<T?> IService<T>.FindAsync(int Id)
        {
            return await _repo.FindAsync(Id);
        }
    }
}
