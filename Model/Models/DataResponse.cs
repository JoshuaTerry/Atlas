using System.Collections.Generic;
using DriveCentric.Core.Interfaces;

namespace DriveCentric.Core.Models
{
    public class DataResponse : IDataResponse
    {
        public long? TotalResults { get; set; }

        public bool IsSuccessful { get; set; } = true;

        public List<string> ErrorMessages { get; set; } = new List<string>();

        public List<string> VerboseErrorMessages { get; set; } = new List<string>();
    }
}