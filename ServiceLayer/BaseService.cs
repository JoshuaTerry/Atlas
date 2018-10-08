using DriveCentric.Utilities.Context;

namespace DriveCentric.ServiceLayer
{
    public abstract class BaseService : BaseWithContextInfoAccessor
    {
        public BaseService(IContextInfoAccessor contextInfoAccessor) : base(contextInfoAccessor) { }
    }
}
