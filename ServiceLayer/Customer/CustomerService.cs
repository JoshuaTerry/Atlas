using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.BusinessLogic.Customer;
using DriveCentric.Model;
using DriveCentric.Utilities.Aspects;
using DriveCentric.Utilities.Context;

namespace DriveCentric.ServiceLayer.Customer
{
    public class CustomerService : BaseService, ICustomerService
    {
        private readonly ICustomerBusinessObject customerBusinessObject;

        public CustomerService(
            IContextInfoAccessor contextInfoAccessor,
            ICustomerBusinessObject customerBusinessObject
            ) : base(contextInfoAccessor)
        {
            this.customerBusinessObject = customerBusinessObject;
        }

        [MonitorAsyncAspect]
        public Task<bool> DeleteCustomerAsync(int id)
        {
            return customerBusinessObject.DeleteCustomerAsync(id);
        }

        [MonitorAsyncAspect]
        public Task<ICustomer> GetCustomerAsync(int id)
        {
            return customerBusinessObject.GetCustomerAsync(id);
        }

        [MonitorAsyncAspect]
        public Task<IEnumerable<ICustomer>> GetCustomersAsync(int? limit = null, int? offset = null, Expression predicate = null)
        {
            return customerBusinessObject.GetCustomersAsync(limit, offset, predicate);
        }
    }
}
