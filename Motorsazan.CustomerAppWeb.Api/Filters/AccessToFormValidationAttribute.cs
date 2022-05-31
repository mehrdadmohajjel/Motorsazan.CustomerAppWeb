using Motorsazan.CustomerAppWeb.Shared.Utilities;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Motorsazan.CustomerAppWeb.Api.Filters
{
    public class AccessToFormValidationAttribute: ActionFilterAttribute
    {
        public string FormCode { get; set; }

        public string SystemCode { get; set; } = Settings.SystemCode;

        private void CheckAccessToFrom(HttpActionContext actionContext)
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
                    var result = UserTools.HasAccessToForm(userId, FormCode, SystemCode);
                    if(!result)
                    {
                        actionContext.Response = actionContext.Request
                            .CreateErrorResponse(HttpStatusCode.Forbidden, "شما مجوز دسترسی به این صفحه را ندارید");
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
            CheckAccessToFrom(actionContext);
            base.OnActionExecuting(actionContext);
        }

        public override Task OnActionExecutingAsync(HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            CheckAccessToFrom(actionContext);
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }
}