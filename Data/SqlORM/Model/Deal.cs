using System;
using DriveCentric.Model;
using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace DriveCentric.Data.SqlORM.Model
{
    public class Deal : IDeal
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
        public Customer InternalCustomer { get; set; }

        [Alias("fkCustomerID")]
        public int CustomerId { get; set; }

        [Ignore]
        public ICustomer Customer
        {
            get
            {
                return InternalCustomer;
            }
            set
            {
                InternalCustomer = (Customer)value;
            }
        }
    }
}
