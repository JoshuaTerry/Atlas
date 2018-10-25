using System;
using System.Collections.Generic;
using System.Text;

namespace DriveCentric.Model
{
    public interface IUser : IBaseModel
    {
        Guid ExternalId { get; set; }
        Guid GalaxyUserId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
    }
}
