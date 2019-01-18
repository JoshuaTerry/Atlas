using System;
using DriveCentric.Core.Interfaces;
using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace DriveCentric.Core.Models
{
    public class Deal : IBaseModel, IStarEntity
    {
        [PrimaryKey]
        [JsonIgnore]
        [Alias("pkDealID")]
        public int Id { get; set; }

        [Alias("GUID")]
        public Guid ExternalId { get; set; }

        [JsonIgnore]
        [Alias("Customer")]
        [Reference]
        public Customer Customer { get; set; }

        [Alias("fkCustomerID")]
        public int CustomerId { get; set; }
    }
}