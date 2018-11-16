using System;
using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace DriveCentric.Model
{
    public class DriveServer : IBaseModel
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
