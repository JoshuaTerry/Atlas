using System;
using DriveCentric.Model;
using DriveCentric.Model.Enums;
using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace DriveCentric.Data.SqlORM.Model
{
    public class Task : ITask
    {
        [PrimaryKey]
        [Alias("pkTaskID")]
        public int Id { get; set; }

        [JsonIgnore]
        [Alias("User")]
        [Reference]
        public User InternalUser { get; set; }

        [Alias("fkUserID")]
        public int UserId { get; set; }

        [Ignore]
        public IUser User
        {
            get
            {
                return InternalUser;
            }
            set
            {
                InternalUser = (User)value;
            }
        }

        [JsonIgnore]
        [Alias("Store")]
        [Reference]
        public Store InternalStore { get; set; }

        [Alias("fkStoreID")]
        public int StoreId { get; set; }

        [Ignore]
        public IStore Store
        {
            get
            {
                return InternalStore;
            }
            set
            {
                InternalStore = (Store)value;
            }
        }

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

        [JsonIgnore]
        [Alias("User")]
        [Reference]
        public User InternalCreatedByUser { get; set; }

        [Alias("fkCreatedByUserID")]
        public int CreatedByUserId { get; set; }

        [Ignore]
        public IUser CreatedByUser
        {
            get
            {
                return InternalCreatedByUser;
            }
            set
            {
                InternalCreatedByUser = (User)value;
            }
        }

        [Alias("Description")]
        public string Notes { get; set; }

        public DateTime? DateDue { get; set; }

        [JsonIgnore]
        [Alias("Deal")]
        [Reference]
        public Deal InternalDeal { get; set; }

        [Alias("fkDealID")]
        public int DealId { get; set; }

        [Ignore]
        public IDeal Deal
        {
            get
            {
                return InternalDeal;
            }
            set
            {
                InternalDeal = (Deal)value;
            }
        }

        public ActionType ActionType { get; set; }

        [Alias("GUID")]
        public Guid ExternalId { get; set; }
    }
}
