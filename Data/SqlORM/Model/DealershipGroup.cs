using System;
using DriveCentric.Model;
using ServiceStack.DataAnnotations;

namespace DriveCentric.Data.SqlORM.Model
{
    [Alias("StoreGroup")]
    public class DealershipGroup : IDealershipGroup
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
