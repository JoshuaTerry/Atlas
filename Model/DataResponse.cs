using System;
using System.Collections.Generic;
using System.Text;

namespace DriveCentric.Model
{
    public class DataResponse : IDataResponse
    {
        public long? TotalResults { get; set; }
        public bool IsSuccessful { get; set; } = true;
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public List<string> VerboseErrorMessages { get; set; } = new List<string>();
    }

    public class DataResponse<T> : DataResponse, IDataResponse<T>
    {
        public T Data { get; set; }

        public DataResponse() : base() { }

        public DataResponse(T data) : this()
        {
            Data = data;
        }
    }
}
