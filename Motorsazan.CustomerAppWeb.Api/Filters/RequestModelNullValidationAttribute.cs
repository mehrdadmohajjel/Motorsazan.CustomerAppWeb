using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Motorsazan.CustomerAppWeb.Api.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequestModelNullValidationAttribute: ActionFilterAttribute
    {
        private readonly Func<Dictionary<string, object>, bool> _validate;

        public RequestModelNullValidationAttribute(): this(arguments => arguments.ContainsValue(null))
        {
        }

        public RequestModelNullValidationAttribute(Func<Dictionary<string, object>, bool> checkCondition) =>
            _validate = checkCondition;

        private void CheckModel(HttpActionContext actionContext)
        {
            if(_validate(actionContext.ActionArguments))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, "The argument cannot be null");
            }
        }

        public override void OnActionExecuting(HttpActionContext actionContext) => CheckModel(actionContext);

        public override Task OnActionExecutingAsync(HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            CheckModel(actionContext);
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }
}