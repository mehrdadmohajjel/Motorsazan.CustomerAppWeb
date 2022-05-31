using Motorsazan.CustomerAppWeb.Api.HttpActionResults;
using Motorsazan.CustomerAppWeb.Api.Utilities;
using Newtonsoft.Json;
using System;
using System.Web.Http.ExceptionHandling;

namespace Motorsazan.CustomerAppWeb.Api.ExceptionHandlers
{
    public class UnhandledExceptionHandler: ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var exceptionMessage = context.Exception.GetBaseException().Message;
#if DEBUG
            var debugContent = JsonConvert.SerializeObject(context.Exception);
#else
            var debugContent =
 @"{ ""Message"" : ""فرآیند اجرا با مشکل مواجه شده است، لطفا با تیم توسعه تماس حاصل فرمایید"" }";
#endif
            Log.Fatal(context.Exception.ToString(), DateTime.Now.Ticks);

            context.Result = new ErrorContentResult(debugContent, "application/json", context.Request);
        }
    }
}