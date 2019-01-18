using System;
using DriveCentric.Core.Interfaces;
using ServiceStack.DataAnnotations;

namespace DriveCentric.Core.Models
{
    [Alias("GalaxyClaimPermissionsView")]
    public class GalaxyClaimPermission : IBaseModel, IGalaxyEntity, IClaimPermission
    {
        public string Key { get; set; }

        public string Value { get; set; }

        [Alias("UserId")]
        public int Id { get; set; }

        [Alias("GUID")]
        public Guid ExternalId { get; set; }
    }
}
