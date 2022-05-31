using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Motorsazan.CustomerAppWeb.Api.Filters
{
    public class RequestModelValidationAttribute: ActionFilterAttribute
    {
        private void CheckModel(HttpActionContext actionContext)
        {
            if(!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request
                    .CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            CheckModel(actionContext);
            base.OnActionExecuting(actionContext);
        }

        public override Task OnActionExecutingAsync(HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            CheckModel(actionContext);
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }
}