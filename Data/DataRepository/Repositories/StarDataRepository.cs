using System;
using DriveCentric.Core.Interfaces;
using DriveCentric.Utilities.Configuration;
using DriveCentric.Utilities.Context;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace DriveCentric.Data.SqlORM.Repositories
{
    public class StarDataRepository<T> : BaseDataRepository<T>, IDataRepository<T>
        where T : IBaseModel, new()
    {
        private readonly IDriveServerCollection driveServerCollection;

        public StarDataRepository(
            IContextInfoAccessor contextInfoAccessor,
            IDriveServerCollection driveServerCollection
            ) : base(contextInfoAccessor)
        {
            this.driveServerCollection = driveServerCollection;
        }

        public override IDbConnectionFactory GetDbFactory()
        {
            try
            {
                const int driveServerId = 21;
                //var driveServerId = Convert.ToInt32(ContextInfoAccessor.ContextInfo.User.Claims.Single(c => c.Type == "custom:DriveServerId"));

                return new OrmLiteConnectionFactory(
                        driveServerCollection.GetConnectionStringById(driveServerId),
                        SqlServerDialect.Provider
                        );
            }
            catch (Exception ex)
            {
                throw new Exception("No DriveServerId was found in the token.", ex);
            }
        }
    }
}
