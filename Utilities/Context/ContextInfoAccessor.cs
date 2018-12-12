using Microsoft.AspNetCore.Http;

namespace DriveCentric.Utilities.Context
{
    public class ContextInfoAccessor : IContextInfoAccessor
    {
        public IContextInfo ContextInfo { get; set; }

        public ContextInfoAccessor(IHttpContextAccessor httpContextAccessor)
        {
            ContextInfo = new ContextInfo(httpContextAccessor);
        }
    }
}