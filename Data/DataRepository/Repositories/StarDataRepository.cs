using System;
using System.Collections.Generic;
using System.Data;
using System.Linq; 
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using DriveCentric.Model;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Configuration;
using DriveCentric.Utilities.Context;
using DriveCentric.Model.Interfaces;
using ServiceStack.OrmLite;
using ServiceStack.Data;

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
