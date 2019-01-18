using DriveCentric.Core.Interfaces;

namespace DriveCentric.Core.Models
{
    public class DataResponseBase<T> : DataResponse, IDataResponse<T>
    {
        public DataResponseBase()
            : base()
        {
        }

        public DataResponseBase(T data)
            : this()
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}