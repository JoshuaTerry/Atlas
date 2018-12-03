using DriveCentric.Core.Interfaces;
using Newtonsoft.Json;
using ServiceStack.DataAnnotations;
using System;

namespace DriveCentric.Core.Models
{
    public class Customer : IBaseModel, IStarEntity
    {
        [PrimaryKey]
        [JsonIgnore]
        [Alias("pkCustomerID")]
        public int Id { get; set; }

        [Alias("GUID")]
        public Guid ExternalId { get; set; }

        [Alias("PrimaryFirstName")]
        public string FirstName { get; set; }

        [Alias("PrimaryLastName")]
        public string LastName { get; set; }
    }
}
