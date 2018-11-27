using DriveCentric.Model;
using DriveCentric.Model.Interfaces;
using DriveCentric.Utilities.Configuration;
using DriveCentric.Utilities.Context;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;

namespace DriveCentric.Data.SqlORM.Repositories
{
    public class StarDataRepository<T> : BaseDataRepository<T> 
        where T : IBaseModel, IStarEntity, new()
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
                var driveServerId = 21;
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
