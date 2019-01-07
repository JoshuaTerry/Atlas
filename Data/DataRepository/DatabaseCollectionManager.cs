using DriveCentric.Core.Interfaces;
using DriveCentric.Data.DataRepository.Interfaces;
using DriveCentric.Data.DataRepository.Repositories;
using DriveCentric.Utilities.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DriveCentric.Data.DataRepository
{
    public class DatabaseCollectionManager : IDatabaseCollectionManager
    {
        private readonly Dictionary<string, IRepository> repositories;

        public DatabaseCollectionManager(Dictionary<string, IRepository> repositories)
        {
            this.repositories = repositories;
        }

        public DatabaseCollectionManager(IContextInfoAccessor contextInfoAccessor, IDriveServerCollection driveServerCollection)
        {
            var driveServerId = Convert.ToInt32(contextInfoAccessor.ContextInfo.User.Claims?.SingleOrDefault(c => c.Type == "custom:DriveServerId")?.Value);

            if (driveServerCollection.ContainsKey(driveServerId))
            {
                repositories = new Dictionary<string, IRepository>
                {
                    { "Galaxy", new SqlRepository(driveServerCollection.GalaxyConnectionString) },
                    { "Star", new SqlRepository(driveServerCollection[driveServerId].ConnectionString) }
                };
            }
            else
            {
                repositories = new Dictionary<string, IRepository>
                {
                    { "Galaxy", new SqlRepository(driveServerCollection.GalaxyConnectionString) }
                };
            }
        }

        public Dictionary<string, IRepository> Repositories => repositories;
    }
}