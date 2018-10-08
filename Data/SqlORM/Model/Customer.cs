using System;
using DriveCentric.Model;
using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace DriveCentric.Data.SqlORM.Model
{
    public class Customer : ICustomer
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

        [JsonIgnore]
        [Alias("fkStoreID")]
        public int DealershipGroupId { get; set; }

        [Ignore]
        public IDealershipGroup DealershipGroup { get; set; }
    }
}

