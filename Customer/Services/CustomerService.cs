using System.Threading.Tasks;
using DriveCentric.BaseService.Services;
using DriveCentric.BusinessLogic.Implementation;
using DriveCentric.Core.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Utilities.Context;

namespace DriveCentric.CustomerService.Services
{
    public class CustomerService : BaseService<Customer>
    {
        public CustomerService(IContextInfoAccessor contextInfoAccessor, IUnitOfWork unitOfWork, LogicBase<Customer> logic) : base(contextInfoAccessor, unitOfWork, logic)
        {
            this.logic = new CustomerLogic(unitOfWork);
        }

        private new CustomerLogic logic;

        public async Task<IDataResponse<long>> MergeCustomers(int sourceCustomerId, int targetCustomerId)
        {
            var entity = await logic.MergeCustomers(sourceCustomerId, targetCustomerId);
            unitOfWork.Update(entity);

            return new DataResponseBase<long> { Data = await unitOfWork.SaveChanges() };
        }
    }
}