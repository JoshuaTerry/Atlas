using DriveCentric.Utilities.Context;

namespace DriveCentric.BusinessLogic
{
    public abstract class BaseBusinessObject : BaseWithContextInfoAccessor
    {
        public BaseBusinessObject(IContextInfoAccessor contextInfoAccessor) : base(contextInfoAccessor) { }
    }
}
