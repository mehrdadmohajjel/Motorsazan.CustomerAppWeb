using Motorsazan.CustomerAppWeb.Shared.Utilities;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Motorsazan.CustomerAppWeb.Api.Filters
{
    public class AccessToEventValidationAttribute: ActionFilterAttribute
    {
        public string FormCode { get; set; }

        public string EventCode { get; set; }

        public string SystemCode { get; set; } = Settings.SystemCode;

        private void CheckAccessToEvent(HttpActionContext actionContext)
        {
            try
            {
                var token = actionContext.Request.Headers.Authorization.Parameter;
                if(string.IsNullOrEmpty(token))
                {
                    actionContext.Response = actionContext.Request
                        .CreateErrorResponse(HttpStatusCode.Unauthorized, "شما مجوز دسترسی به این بخش را ندارید");
                }
                else
                {
                    var userId = UserTools.GetUserIdByJsonWebToken(token);
                    var result = UserTools.HasAccessToEvent(userId, FormCode, EventCode, SystemCode);
                    if(!result)
                    {
                        actionContext.Response = actionContext.Request
                            .CreateErrorResponse(HttpStatusCode.Forbidden, "شما مجوز دسترسی به این عملیات را ندارید");
                    }
                }
            }
            catch
            {
                actionContext.Response = actionContext.Request
                    .CreateErrorResponse(HttpStatusCode.Unauthorized, "شما مجوز دسترسی به این بخش را ندارید");
            }
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            CheckAccessToEvent(actionContext);
            base.OnActionExecuting(actionContext);
        }

        public override Task OnActionExecutingAsync(HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            CheckAccessToEvent(actionContext);
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }
}