using DriveCentric.Core.Interfaces;
using FluentValidation;

namespace DriveCentric.BusinessLogic.Implementation
{
    public abstract class LogicBase<T> : AbstractValidator<T>
    {
        private readonly IReadOnlyUnitOfWork unitOfWork;

        protected LogicBase(IReadOnlyUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        public IReadOnlyUnitOfWork UnitOfWork => unitOfWork;
    }
}