using NTierOracleIdentityExample.Dll.Context;
using NTierOracleIdentityExample.Dll.Entities;
using NTierOracleIdentityExample.Dll.Repositories.Abstract;

namespace NTierOracleIdentityExample.Dll.Repositories.Implementation
{
    public class LogRepository : BaseRepository<Log>, ILogRepository
    {
        //Implement additional methods or override ones from BaseRepository and implement them
        #region Select methods
        public LogRepository(EXAMPLE_SCHEMA_Context context) : base(context)
        { }
        #endregion

        #region Select methods

        #endregion

        #region Insert methods


        #endregion

        #region Update methods

        #endregion

        #region Delete methods

        #endregion

        #region Other methods

        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion
    }
}
