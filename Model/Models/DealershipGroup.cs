using System;
using DriveCentric.Core.Interfaces;
using ServiceStack.DataAnnotations;

namespace DriveCentric.Core.Models
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