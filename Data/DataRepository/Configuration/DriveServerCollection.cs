using DriveCentric.Core.Models;
using DriveCentric.Data.DataRepository.Interfaces;
using Microsoft.Extensions.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System.Collections.Generic;
using System.Linq;

namespace DriveCentric.Data.DataRepository.Configuration
{
    public class DriveServerCollection : Dictionary<int, DriveServer>, IDriveServerCollection
    {
        private readonly IDbConnectionFactory factory;

        public string GalaxyConnectionString { get; private set; }

        public DriveServerCollection(IConfiguration configuration)
        {
            GalaxyConnectionString = configuration.GetSection("SqlDBInfo:ConnectionString").Value;
            factory = new OrmLiteConnectionFactory(GalaxyConnectionString, SqlServerDialect.Provider);

            using (var connection = factory.OpenDbConnection())
            {
                foreach (var kvp in connection.Select<DriveServer>().ToDictionary(server => server.Id))
                {
                    this.Add(kvp.Key, kvp.Value);
                }
            }
        }
    }
}