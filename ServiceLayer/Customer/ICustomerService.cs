using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DriveCentric.Model;

namespace DriveCentric.ServiceLayer.Customer
{
    public interface ICustomerService
    {
        Task<ICustomer> GetCustomerAsync(int id);
        Task<IEnumerable<ICustomer>> GetCustomersAsync(int? limit = null, int? offset = null, Expression predicate = null);
        Task<bool> DeleteCustomerAsync(int id);
    }
}
