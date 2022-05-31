using Motorsazan.CustomerAppWeb.Api.Utilities;
using System;
using System.Web.Http;
using System.Web.Mvc;

namespace Motorsazan.CustomerAppWeb.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Log.Information("Application started", DateTime.Now.Ticks);

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            AreaRegistration.RegisterAllAreas();
        }

        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            Utilities.Log.Error(ex.ToString(), DateTime.Now.Ticks);
        }
    }
}
