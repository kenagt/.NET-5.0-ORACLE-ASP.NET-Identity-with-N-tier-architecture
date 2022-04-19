using System.Collections.Generic;
using System.Threading.Tasks;

namespace NTierOracleIdentityExample.Dll.Repositories.Abstract
{
    public interface IBaseRepository<T> where T : class
    {
        #region Select methods
        Task<T> SelectById(int id);
        Task<List<T>> SelectAll();
        #endregion

        #region Insert methods
        Task<T> Insert(T entity);
        #endregion

        #region Update methods
        void Update(T entity);
        #endregion

        #region Delete methods
        void Delete(int Id);
        #endregion

        #region Other methods
        void SaveChanges();
        #endregion
    }
}
