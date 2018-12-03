using System;
using DriveCentric.Utilities.Context;
using PostSharp.Aspects;
using Serilog;

namespace DriveCentric.Utilities.Aspects
{
    [Serializable]
    public class MonitorAspect : MethodInterceptionAspect
    {
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            var instance = args.Instance as IContextAccessible;

            using (Log.ForContext(args.Instance.GetType())
                .BeginTimedOperation(
                    args.Instance.GetType() + "." + args.Method.Name,
                    instance?.ContextInfoAccessor?.ContextInfo?.Identifier.ToString()))
            {
                args.Proceed();
            }
        }
    }
}
