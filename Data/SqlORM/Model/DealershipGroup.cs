using System;
using System.Collections.Generic;
using System.Text;
using DriveCentric.Model;
using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace DriveCentric.Data.SqlORM.Model
{
    [Alias("StoreGroup")]
    public class DealershipGroup : IDealershipGroup
    {
        [PrimaryKey]
        [JsonIgnore]
        [Alias("pkGroupID")]
        public int Id { get; set; }

        [Alias("GUID")]
        public Guid ExternalId { get; set; }

        [Alias("Name")]
        public string Name { get; set; }
    }
}
