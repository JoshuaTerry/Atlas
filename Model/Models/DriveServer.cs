using System;
using DriveCentric.Core.Interfaces;
using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace DriveCentric.Core.Models
{
    public class DriveServer : IBaseModel, IGalaxyEntity
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
