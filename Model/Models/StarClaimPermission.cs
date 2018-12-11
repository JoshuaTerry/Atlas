using System;
using System.Collections.Generic;
using System.Text;
using DriveCentric.Core.Interfaces;
using ServiceStack.DataAnnotations;

namespace DriveCentric.Core.Models
{
    [Alias("StarClaimPermissionsView")]
    public class StarClaimPermission : IBaseModel, IStarEntity, IClaimPermission
    {
        public string Key { get; set; }
        public string Value { get; set; }

        [Alias("UserId")]
        public int Id { get; set; }

        [Alias("GUID")]
        public Guid ExternalId { get; set; }
    }
}
