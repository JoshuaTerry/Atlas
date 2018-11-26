using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace DriveCentric.Model
{
    public class Store : IBaseModel
    {
        [PrimaryKey]
        [JsonIgnore]
        [Alias("pkStoreID")]
        public int Id { get; set; }

        [Alias("GUID")]
        public Guid ExternalId { get; set; }
    }
}
