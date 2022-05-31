using System.Web.Http;

namespace Motorsazan.CustomerAppWeb.Client
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config) => config.Routes.MapHttpRoute(
            "DefaultApi",
            "api/{controller}/{id}",
            new {id = RouteParameter.Optional}
        );
    }
}