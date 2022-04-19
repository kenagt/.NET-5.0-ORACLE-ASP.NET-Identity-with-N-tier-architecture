using Microsoft.EntityFrameworkCore;
using NTierOracleIdentityExample.Dll.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NTierOracleIdentityExample.Dll.Repositories.Implementation
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        #region Fields
        protected readonly DbContext _context;
        protected readonly DbSet<T> dbSet;
        #endregion

        #region Constructor
        protected BaseRepository(DbContext context)
        {
            _context = context;
            dbSet = context.Set<T>();
        }
        #endregion

        #region Select methods

        public virtual async Task<T> SelectById(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<List<T>> SelectAll()
        {
            return await dbSet.ToListAsync();
        }

        #endregion

        #region Insert methods
        public virtual async Task<T> Insert(T entity)
        {
            var addedEntity = (await dbSet.AddAsync(entity)).Entity;
            await _context.SaveChangesAsync();

            return addedEntity;
        }

        #endregion

        #region Update methods
        public virtual async void Update(T entity)
        {
            dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Delete methods
        public async void Delete(int Id)
        {
            T entity = dbSet.Find(Id);
            var removedEntity = dbSet.Remove(entity).Entity;
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Other methods
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        #endregion
    }
}
