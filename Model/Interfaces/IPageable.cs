using System;
using System.Collections.Generic;
using System.Text;

namespace DriveCentric.Model
{
    public interface IPageable
    {
        int? Offset { get; set; }
        int? Limit { get; set; }
        string OrderBy { get; set; }
    }
}
