using System;
using DriveCentric.Model;
using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace DriveCentric.Data.SqlORM.Model
{
    public class DriveServer : IDriveServer
    {
        [PrimaryKey]
        [JsonIgnore]
        [Alias("pkDriveServerID")]
        public int Id { get; set; }

        [Alias("GUID")]
        public Guid ExternalId { get; set; }

        [Alias("ConnectionString")]
        public string ConnectionString { get; set; }
    }
}
