using System.Collections.Generic;
using System.Linq;
using DriveCentric.Core.Models;
using DriveCentric.Data.DataRepository.Interfaces;
using Microsoft.Extensions.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace DriveCentric.Data.DataRepository.Configuration
{
    public class DriveServerCollection : IDriveServerCollection
    {
        private readonly IDbConnectionFactory factory;
        private readonly Dictionary<int, DriveServer> servers;

        public string GalaxyConnectionString { get; private set; }

        public DriveServerCollection(IConfiguration configuration)
        {
            GalaxyConnectionString = configuration.GetSection("SqlDBInfo:ConnectionString").Value;
            factory = new OrmLiteConnectionFactory(GalaxyConnectionString, SqlServerDialect.Provider);
            using (var connection = factory.OpenDbConnection())
            {
                servers = connection.Select<DriveServer>().ToDictionary(server => server.Id);
            }
        }

        public string GetConnectionStringById(int id)
        {
            var server = servers.FirstOrDefault(item => item.Key == id).Value;

            if (server != null)
            {
                return server.ConnectionString;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }
    }
}
