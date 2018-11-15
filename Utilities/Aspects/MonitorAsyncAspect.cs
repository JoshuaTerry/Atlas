using System;
using System.Threading.Tasks;
using DriveCentric.Utilities.Context;
using PostSharp.Aspects;
using Serilog;

namespace DriveCentric.Utilities.Aspects
{
    [Serializable]
    public class MonitorAsyncAspect : MethodInterceptionAspect
    {        
        public override async Task OnInvokeAsync(MethodInterceptionArgs args)
        {
            var instance = args.Instance as IContextAccessible;

            using (Log.ForContext(args.Instance.GetType())
                .BeginTimedOperation(
                    args.Instance.GetType() + "." + args.Method.Name,
                    instance?.ContextInfoAccessor?.ContextInfo?.Identifier.ToString()))
            {
                await args.ProceedAsync();
            }
        }
    }

    //[Serializable]
    //public class TestAsyncAspect : MethodInterceptionAspect
    //{
    //    public override async Task OnInvokeAsync(MethodInterceptionArgs args)
    //    {
    //        var instance = args.Instance as IContextAccessible;

    //        var response = new DataResponse<IEnumerable<T>>();
    //        try
    //        {
    //            await args.ProceedAsync();
    //        }
    //        catch (Exception ex)
    //        {
    //            response = ProcessDataResponseException<IEnumerable<T>>(ex);
    //        }
    //        return response;

    //        //using (Log.ForContext(args.Instance.GetType())
    //        //    .BeginTimedOperation(
    //        //        args.Instance.GetType() + "." + args.Method.Name,
    //        //        instance?.ContextInfoAccessor?.ContextInfo?.Identifier.ToString()))
    //        //{
    //        //    await args.ProceedAsync();
    //        //}
    //    }
    //}
}
