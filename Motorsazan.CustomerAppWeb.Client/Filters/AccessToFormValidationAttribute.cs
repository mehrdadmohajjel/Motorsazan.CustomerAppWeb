using Motorsazan.CustomerAppWeb.Shared.Utilities;
using System.Web.Mvc;

namespace Motorsazan.CustomerAppWeb.Client.Filters
{
    public class AccessToFormValidationAttribute: ActionFilterAttribute
    {
        public string FormCode { get; set; }

        public string SystemCode { get; set; } = Settings.SystemCode;

        private void CheckAccessToForm(ActionExecutingContext filterContext)
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
                    var result = UserTools.HasAccessToForm(userId, FormCode, SystemCode);
                    if(!result)
                    {
                        filterContext.Result = new RedirectResult("~/Forbidden");
                    }
                }
            }
            catch
            {
                filterContext.Result = new RedirectResult("~/Forbidden");
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CheckAccessToForm(filterContext);
            base.OnActionExecuting(filterContext);
        }
    }
}