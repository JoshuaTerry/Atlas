using ServiceStack.DataAnnotations;
using System;

namespace DriveCentric.Model
{
    [Alias("StoreGroup")]
    public class DealershipGroup : IBaseModel
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
