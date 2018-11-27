using DriveCentric.Model.Interfaces;
using ServiceStack.DataAnnotations;
using System;

namespace DriveCentric.Model
{
    [Alias("StoreGroup")]
    public class DealershipGroup : IBaseModel, IStarEntity
    {
        [PrimaryKey]
        [Alias("fkGroupID")]
        public int Id { get; set; }

        [Alias("GUID")]
        public Guid ExternalId { get; set; }

        [Alias("Name")]
        public string Name { get; set; }
    }
} 
