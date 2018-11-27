using DriveCentric.Core.Interfaces;
using DriveCentric.Utilities.Context;
using ServiceStack.Data;

namespace DriveCentric.Data.SqlORM.Repositories
{
    public class GalaxyDataRepository<T> : BaseDataRepository<T>, IDataRepository<T>
        where T : IBaseModel, new()
    {
        protected readonly IDbConnectionFactory dbFactory;

        public GalaxyDataRepository(
            IContextInfoAccessor contextInfoAccessor,
            IDbConnectionFactory dbFactory
            ) : base(contextInfoAccessor)
        {
            this.dbFactory = dbFactory;
        }

        public override IDbConnectionFactory GetDbFactory()
        {
            return dbFactory;
        }
    }
}
