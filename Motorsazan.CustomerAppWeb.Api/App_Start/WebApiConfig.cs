using Motorsazan.CustomerAppWeb.Api.ExceptionHandlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Motorsazan.CustomerAppWeb.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            var json = config.Formatters.JsonFormatter.SerializerSettings;
            json.ContractResolver = new CamelCasePropertyNamesContractResolver();
            json.Formatting = Formatting.Indented;
            json.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            config.Services.Replace(typeof(IExceptionHandler), new UnhandledExceptionHandler());
        }
    }
}