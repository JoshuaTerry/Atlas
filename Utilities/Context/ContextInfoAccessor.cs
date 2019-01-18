using Microsoft.AspNetCore.Http;

namespace DriveCentric.Utilities.Context
{
    public class ContextInfoAccessor : IContextInfoAccessor
    {
        public ContextInfoAccessor(IHttpContextAccessor httpContextAccessor)
        {
            ContextInfo = new ContextInfo(httpContextAccessor);
        }

        public IContextInfo ContextInfo { get; set; }
    }
}