using DriveCentric.Core.Interfaces;
using DriveCentric.Core.Validators;
using DriveCentric.Model.Enums;
using Newtonsoft.Json;
using ServiceStack.DataAnnotations;
using System;

namespace DriveCentric.Core.Models
{
    public class UserTask : IBaseModel, IStarEntity
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

        [DateValidator(ErrorMessage = "Due date must be greater than current Date")]
        public DateTime? DateDue { get; set; }

        [JsonIgnore]
        [Alias("Deal")]
        [Reference]
        public Deal Deal { get; set; }

        [Alias("fkDealID")]
        public int DealId { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [ServiceStack.DataAnnotations.Required]
        //[EnumDataType(typeof(ActionType))]
        public ActionType? ActionType { get; set; }

        [Alias("GUID")]
        public Guid ExternalId { get; set; }
    }
}