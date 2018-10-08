using System;

namespace DriveCentric.Model
{
    public interface IBaseExternalIdModel : IBaseModel
    {
        Guid ExternalId { get; set; }
    }
}
