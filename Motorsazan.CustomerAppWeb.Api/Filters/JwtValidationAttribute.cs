using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Motorsazan.CustomerAppWeb.Api.Filters
{
    public class JwtValidationAttribute: ActionFilterAttribute
    {
        private void CheckToken(HttpActionContext actionContext)
        {
            try
            {
                if(actionContext.Request.Method.Method.ToUpper().Equals("OPTIONS"))
                {
                    return;
                }

                var token = actionContext.Request.Headers.Authorization.Parameter;
                if(string.IsNullOrEmpty(token))
                {
                    actionContext.Response = actionContext.Request
                        .CreateErrorResponse(HttpStatusCode.Unauthorized, "شما مجوز دسترسی به این بخش را ندارید");
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
            CheckToken(actionContext);
            base.OnActionExecuting(actionContext);
        }

        public override Task OnActionExecutingAsync(HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            CheckToken(actionContext);
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }
}