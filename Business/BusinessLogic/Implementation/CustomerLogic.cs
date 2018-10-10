using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Model;
using DriveCentric.Utilities.Context;
using DriveCentric.Utilities.Data;

namespace DriveCentric.BusinessLogic.Implementation
{
    public class CustomerLogic : BaseLogic<ICustomer>, ICustomerLogic
    {
       public CustomerLogic(
            IContextInfoAccessor contextInfoAccessor,
            IDataRepository<ICustomer> dataRepository
            ) : base(contextInfoAccessor, dataRepository)
        { }
    }
}
