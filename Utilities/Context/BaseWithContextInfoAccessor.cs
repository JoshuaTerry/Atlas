namespace DriveCentric.Utilities.Context
{
    public abstract class BaseWithContextInfoAccessor : IContextAccessible
    {
        public IContextInfoAccessor ContextInfoAccessor { get; }

        protected BaseWithContextInfoAccessor(IContextInfoAccessor contextInfoAccessor)
        {
            ContextInfoAccessor = contextInfoAccessor;
        }
    }
}
