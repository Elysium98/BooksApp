using BooksApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksApp.Data.Implementation
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDBContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        public BaseRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
        public virtual async Task CreateAsync(T entity) => await _dbSet.AddAsync(entity);
        public virtual async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();
        public virtual void Update(T entity) => _dbSet.Update(entity);
        public virtual void Delete(T entity) => _dbSet.Remove(entity);
        public virtual Task<T> GetByIdAsync(object id) => _dbSet.FindAsync(id).AsTask();
        public virtual Task<int> SaveChangesAsync() => _dbContext.SaveChangesAsync();

    }
}
