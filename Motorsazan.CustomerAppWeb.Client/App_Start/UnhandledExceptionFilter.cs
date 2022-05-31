using Motorsazan.CustomerAppWeb.Shared.Models.Base;
using System.Web.Mvc;

namespace Motorsazan.CustomerAppWeb.Client
{
    public class CustomExceptionHandlerFilter: FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            var exception = filterContext.Exception.GetBaseException();

            var code = "unknown";
            if(exception is LoginException)
            {
                code = "auth001";
            }
            else if(exception is AlertException)
            {
                code = "alert001";
            }

            filterContext.Result = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new {message = exception.Message, code}
            };
        }
    }
}