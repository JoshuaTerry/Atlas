using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.Model;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using DriveCentric.Utilities.Data;

namespace DriveCentric.BusinessLogic.Deal
{
    public class DealBusinessObject : BaseBusinessObject, IDealBusinessObject
    {
        private readonly IDataRepository<IDeal> dataRepository;

        public DealBusinessObject(
            IContextInfoAccessor contextInfoAccessor,
            IDataRepository<IDeal> dataRepository
            ) : base(contextInfoAccessor)
        {
            this.dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));
        }

        [MonitorAsyncAspect]
        public Task<bool> DeleteDealAsync(int id)
        {
            return dataRepository.DeleteByIdAsync(id);
        }

        [MonitorAsyncAspect]
        public Task<IDeal> GetDealAsync(int id)
        {
            return dataRepository.GetByIdAsync(id);
        }

        [MonitorAsyncAspect]
        public Task<IEnumerable<IDeal>> GetDealsAsync(
            int? limit = null,
            int? offset = null,
            Expression predicate = null)
        {
            return dataRepository.GetAsync(limit, offset, predicate);
        }
    }
}
