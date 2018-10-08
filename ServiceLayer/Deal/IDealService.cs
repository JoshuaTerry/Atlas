using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.Model;

namespace DriveCentric.ServiceLayer.Deal
{
    public interface IDealService
    {
        Task<IDeal> GetDeal(int id);

        Task<IEnumerable<IDeal>> GetCustomersAsync(
            int? limit = null,
            int? offset = null,
            Expression predicate = null);

        Task<bool> DeleteDeal(int id);
    }
}
