using System.Collections.Generic;
using System.Threading.Tasks;

namespace DriveCentric.Core.Interfaces
{
    public interface IUnitOfWork : IReadOnlyUnitOfWork
    {
        Task<Dictionary<string, bool>> GetDatabaseHealthCheck();

        Task<long> SaveChanges();

        void Delete<T>(int id) where T : IBaseModel, new();

        void Update<T>(T entity) where T : IBaseModel, new();

        void Insert<T>(T entity) where T : IBaseModel, new();
    }
}