using System;
using System.Collections.Generic;
using System.Text;
using DriveCentric.Model.Enums;

namespace DriveCentric.Model
{
    public interface ITask : IBaseModel
    {
        int CustomerId { get; set; }
        ICustomer Customer { get; set; }

        int CreatedByUserId { get; set; }
        IUser CreatedByUser { get; set; }

        int UserId { get; set; }
        IUser User { get; set; }

        ActionType ActionType { get; set; }
        DateTime? DateDue { get; set; }
        string Notes { get; set; }
        Guid ExternalId { get; set; }
    }
}
