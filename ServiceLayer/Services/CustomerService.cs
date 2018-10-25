using DriveCentric.BaseService.Services;
using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.ServiceLayer.Interfaces;
using DriveCentric.Utilities.Context;

namespace DriveCentric.ServiceLayer.Services
{
    public class CustomerService : BaseService<ICustomer>, ICustomerService
    {
        public CustomerService(
            IContextInfoAccessor contextInfoAccessor,
            ICustomerLogic businessLogic
            ) : base(contextInfoAccessor, businessLogic)
        { }
    }
}
