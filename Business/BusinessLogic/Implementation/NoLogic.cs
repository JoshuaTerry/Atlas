using DriveCentric.Core.Interfaces;

namespace DriveCentric.BusinessLogic.Implementation
{
    public class NoLogic<T> : LogicBase<T>
    {
        public NoLogic(IReadOnlyUnitOfWork uow) : base(uow)
        { }
    }
}