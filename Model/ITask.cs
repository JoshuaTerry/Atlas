using System;
using System.Collections.Generic;
using System.Text;
using DriveCentric.Model.Enums;

namespace DriveCentric.Model
{
    public interface ITask : IBaseModel
    {
        ICustomer Customer { get; set; }
        IUser CreatedByUser { get; set; }
        IUser User { get; set; }
        ActionType ActionType { get; set; }
        DateTime? DateDue { get; set; }
        string Notes { get; set; }
    }
}
