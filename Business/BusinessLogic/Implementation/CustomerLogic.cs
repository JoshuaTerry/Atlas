using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.Utilities.Context;
using DriveCentric.Model.Interfaces;

namespace DriveCentric.BusinessLogic.Implementation
{
    public class CustomerLogic : BaseLogic<Customer>, ICustomerLogic
    {
       public CustomerLogic(
            IContextInfoAccessor contextInfoAccessor,
            IDataRepository<Customer> dataRepository
            ) : base(contextInfoAccessor, dataRepository)
        { }
    }
}
