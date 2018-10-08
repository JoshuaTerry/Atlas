using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.Model;
using Microsoft.Extensions.Configuration;

namespace DriveCentric.Utilities.Data
{
    public interface IDataAccessor
    {
        IEnumerable<T> Get<T>(
            IDbConnection connection,
            int? limit = null,
            int? offset = null,
            Expression<Func<T, bool>> predicate = null)
            where T : IBaseExternalIdModel;

        Task<IEnumerable<T>> GetAsync<T>(
            IDbConnection connection,
            int? limit = null,
            int? offset = null,
            Expression<Func<T, bool>> predicate = null)
            where T: IBaseExternalIdModel;

        Task<T> GetByIdAsync<T>(int id, IDbConnection connection, string query = null)
            where T : IBaseExternalIdModel;

        Task<int> DeleteByIdAsync<T>(int id, IDbConnection connection, string query = null)
            where T : IBaseExternalIdModel;
    }
}
