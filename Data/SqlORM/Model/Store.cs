using System;
using DriveCentric.Model;
using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace DriveCentric.Data.SqlORM.Model
{
    public class Store : IStore
    {
        [PrimaryKey]
        [JsonIgnore]
        [Alias("pkStoreID")]
        public int Id { get; set; }

        [Alias("GUID")]
        public Guid ExternalId { get; set; }
    }
}
