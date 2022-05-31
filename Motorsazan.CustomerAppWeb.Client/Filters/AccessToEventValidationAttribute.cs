using Motorsazan.CustomerAppWeb.Shared.Utilities;
using System.Net;
using System.Web.Mvc;

namespace Motorsazan.CustomerAppWeb.Client.Filters
{
    public class AccessToEventValidationAttribute: ActionFilterAttribute
    {
        public string FormCode { get; set; }

        public string EventCode { get; set; }

        public string SystemCode { get; set; } = Settings.SystemCode;

        private void CheckAccessToEvent(ActionExecutingContext filterContext)
        {
            try
            {
                var tokenInHeader = filterContext.HttpContext.Request.Headers.Get("Authorization");
                var tokenInCookie = filterContext.HttpContext.Request.Cookies.Get("MotorsazanJsonWebToken")?.Value;
                if(string.IsNullOrEmpty(tokenInHeader) && string.IsNullOrEmpty(tokenInCookie))
                {
                    filterContext.Result = new RedirectResult("http://erp-server");
                }
                else
                {
                    var token = !string.IsNullOrEmpty(tokenInCookie) ? tokenInCookie : tokenInHeader;

                    var userId = UserTools.GetUserIdByJsonWebToken(token);
                    var result = UserTools.HasAccessToEvent(userId, FormCode, EventCode, SystemCode);
                    if(result)
                    {
                        return;
                    }

                    filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.Forbidden,
                        "شما مجوز دسترسی به این عملیات را ندارید");
                }
            }
            catch
            {
                filterContext.Result = new RedirectResult("~/Forbidden");
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CheckAccessToEvent(filterContext);
            base.OnActionExecuting(filterContext);
        }
    }
}