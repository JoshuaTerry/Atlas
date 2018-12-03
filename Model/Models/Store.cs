using System;
using System.Collections.Generic;
using System.Text;
using DriveCentric.Core.Interfaces;
using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace DriveCentric.Core.Models
{
    public class Store : IBaseModel, IStarEntity
    {
        [PrimaryKey]
        [JsonIgnore]
        [Alias("pkStoreID")]
        public int Id { get; set; }

        [Alias("GUID")]
        public Guid ExternalId { get; set; }
    }
}
