using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.Model;

namespace DriveCentric.BusinessLogic.Deal
{
    public interface IDealBusinessObject
    {
        Task<IDeal> GetDealAsync(int id);

        Task<IEnumerable<IDeal>> GetDealsAsync(
            int? limit = null,
            int? offset = null,
            Expression predicate = null);

        Task<bool> DeleteDealAsync(int id);
    }
}
