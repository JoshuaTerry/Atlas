using System.Collections.Generic;
using System.Linq;
using DriveCentric.Model;
using DriveCentric.Utilities.Configuration;
using DriveCentric.Utilities.Data;

namespace DriveCentric.Data.SqlORM.Configuration
{
    public class DriveServerCollection : IDriveServerCollection
    {
        private readonly Dictionary<int, DriveServer> servers;

        public DriveServerCollection(IDataRepository<DriveServer> driveServerRepository)
        {
            servers = driveServerRepository.Get().ToDictionary(server => server.Id);
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
