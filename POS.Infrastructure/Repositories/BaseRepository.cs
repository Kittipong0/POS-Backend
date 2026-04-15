using Microsoft.EntityFrameworkCore;
using POS.Infrastructure.Data;
using POS.Infrastructure.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace POS.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _writeContext;
        protected readonly AppReadDbContext _readContext;
        protected readonly DbSet<T> _dbSet;

        protected BaseRepository(AppDbContext writeContext, AppReadDbContext readContext)
        {
            _writeContext = writeContext;
            _readContext = readContext;
            _dbSet = _writeContext.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _readContext.Set<T>().AsNoTracking().ToList();
        }

        public virtual T? GetById(int id)
        {
            return _readContext.Set<T>().Find(id);
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            // Robust update pattern to handle tracking conflicts
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null)
            {
                var idValue = idProperty.GetValue(entity);
                var existing = _dbSet.Local.FirstOrDefault(e => {
                    var eval = idProperty.GetValue(e);
                    return eval != null && eval.Equals(idValue);
                });

                if (existing != null)
                {
                    _writeContext.Entry(existing).CurrentValues.SetValues(entity);
                    return;
                }
            }

            _writeContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
