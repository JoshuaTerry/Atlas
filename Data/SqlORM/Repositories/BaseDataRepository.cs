using DriveCentric.Utilities.Context;
using DriveCentric.Utilities.Data;
using ServiceStack.Data;

namespace DriveCentric.Data.SqlORM.Repositories
{
    public abstract class BaseDataRepository : BaseWithContextInfoAccessor
    {
        protected readonly IDataAccessor dataAccessor;

        protected BaseDataRepository(
            IContextInfoAccessor contextInfoAccessor,
            IDataAccessor dataAccessor
            ) : base(contextInfoAccessor)
        {
            this.dataAccessor = dataAccessor;
        }
    }
}
