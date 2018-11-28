using System;
using System.Collections.Generic;
using System.Text;
using DriveCentric.Core.Interfaces;
using DriveCentric.Model.Enums;
using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace DriveCentric.Core.Models
{
    public class Task : IBaseModel
    {
        [PrimaryKey]
        [Alias("pkTaskID")]
        [AutoIncrement]
        public int Id { get; set; }

        [JsonIgnore]
        [Alias("User")]
        [Reference]
        public User User { get; set; }

        [Alias("fkUserID")]
        public int UserId { get; set; }

        [JsonIgnore]
        [Alias("Store")]
        [Reference]
        public Store Store { get; set; }

        [Alias("fkStoreID")]
        public int StoreId { get; set; }

        [JsonIgnore]
        [Alias("Customer")]
        [Reference]
        public Customer Customer { get; set; }

        [Alias("fkCustomerID")]
        public int CustomerId { get; set; }

        [JsonIgnore]
        [Alias("User")]
        [Reference]
        public User CreatedByUser { get; set; }

        [Alias("fkCreatedByUserID")]
        public int CreatedByUserId { get; set; }

        [Alias("Description")]
        public string Notes { get; set; }

        public DateTime? DateDue { get; set; }

        [JsonIgnore]
        [Alias("Deal")]
        [Reference]
        public Deal Deal { get; set; }

        [Alias("fkDealID")]
        public int DealId { get; set; }

        public ActionType ActionType { get; set; }

        [Alias("GUID")]
        public Guid ExternalId { get; set; }
    }
}
