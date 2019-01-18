using System;
using System.Collections.Generic;
using System.Text;

namespace DriveCentric.Core.Interfaces
{
    public interface IClaimPermission
    {
        string Key { get; set; }

        string Value { get; set; }
    }
}
