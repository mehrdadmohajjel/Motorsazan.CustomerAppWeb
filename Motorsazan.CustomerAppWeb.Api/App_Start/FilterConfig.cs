using System.Web.Mvc;

namespace Motorsazan.CustomerAppWeb.Api
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) =>
            filters.Add(new HandleErrorAttribute());
    }
}