using System.Threading.Tasks;
using DriveCentric.Core.Interfaces;
using DriveCentric.Core.Models;
using FluentValidation;

namespace DriveCentric.BusinessLogic.Implementation
{
    public class CustomerLogic : LogicBase<Customer>
    {
        public CustomerLogic(IReadOnlyUnitOfWork uow)
            : base(uow)
        {
            RuleSet("Insert", () =>
            {
                RuleFor(x => x.FirstName).NotNull();
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.Id).NotNull();
            });

            RuleSet("Delete", () =>
            {
                RuleFor(x => x.Id).NotEqual(0);
            });
        }

        public async Task<Customer> MergeCustomers(int sourceCustomerId, int targetCustomerId)
        {
            // Logic here to get customers, determine mergability, return the merged customer

            return await Task<Customer>.FromResult(new Customer());
        }
    }
}