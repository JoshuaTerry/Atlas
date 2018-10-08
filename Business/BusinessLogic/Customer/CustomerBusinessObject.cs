using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.Model;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;
using DriveCentric.Utilities.Data;

namespace DriveCentric.BusinessLogic.Customer
{
    public class CustomerBusinessObject : BaseBusinessObject, ICustomerBusinessObject
    {
        private readonly IDataRepository<ICustomer> customerDataRepository;

        public CustomerBusinessObject(
            IContextInfoAccessor contextInfoAccessor,
            IDataRepository<ICustomer> customerDataRepository
            ) : base(contextInfoAccessor)
        {
            this.customerDataRepository = customerDataRepository ?? throw new ArgumentNullException(nameof(customerDataRepository));
        }

        [MonitorAsyncAspect]
        public Task<bool> DeleteCustomerAsync(int id)
        {
            return customerDataRepository.DeleteByIdAsync(id);
        }

        [MonitorAsyncAspect]
        public Task<ICustomer> GetCustomerAsync(int id)
        {
            // I'm leaving this here to show that we can get to the claims for if/when we
            // want to do some claims-based authorization.
            var user = ContextInfoAccessor?.ContextInfo?.User;

            return customerDataRepository.GetByIdAsync(id);
        }

        [MonitorAsyncAspect]
        public Task<IEnumerable<ICustomer>> GetCustomersAsync(
            int? limit = null,
            int? offset = null,
            Expression predicate = null)
        {
            return customerDataRepository.GetAsync(limit, offset);
        }
    }
}
