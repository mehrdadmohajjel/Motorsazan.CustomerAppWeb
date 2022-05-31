using System.Web.Mvc;

namespace Motorsazan.CustomerAppWeb.Client
{
    public class JsonResultFilter: IResultFilter
    {
        public int MaxJsonLength { get; set; } = int.MaxValue;

        public int? RecursionLimit { get; set; }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if(!(filterContext.Result is JsonResult jsonResult))
            {
                return;
            }

            jsonResult.MaxJsonLength = jsonResult.MaxJsonLength ?? MaxJsonLength;
            jsonResult.RecursionLimit = jsonResult.RecursionLimit ?? RecursionLimit;
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }
    }
}