using System.Collections.Generic;
using System.Linq;
using DriveCentric.Core.Models;
using DriveCentric.Data.DataRepository.Interfaces;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace DriveCentric.Data.DataRepository.Configuration
{
    public class DriveServerCollection : Dictionary<int, DriveServer>, IDriveServerCollection
    {
        public DriveServerCollection(IDbConnectionFactory factory)
        {
            GalaxyConnectionString = factory.CreateDbConnection().ConnectionString;
            using (var connection = factory.OpenDbConnection())
            {
                foreach (var kvp in connection.Select<DriveServer>().ToDictionary(server => server.Id))
                {
                    this.Add(kvp.Key, kvp.Value);
                }
            }
        }

        public string GalaxyConnectionString { get; private set; }
    }
}