using DevExpress.Web;
using DevExpress.Web.Mvc;
using Motorsazan.CustomerAppWeb.Client.Utilities;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Motorsazan.CustomerAppWeb.Client
{
    public class MvcApplication: HttpApplication
    {
        protected void Application_Start()
        {
            Log.Information("Application started", DateTime.Now.Ticks);

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ConfigViewEngins();
            ConfigDevexpress();
        }

        protected void Application_Error()
        {
            var error = Server.GetLastError();
            Log.Fatal(error.ToString(), DateTime.Now.Ticks);
        }

        private void ConfigDevexpress()
        {
            ModelBinders.Binders.DefaultBinder = new DevExpressEditorsBinder();
            ASPxWebControl.CallbackError += HandleDevexpressErrors;
        }

        private void ConfigViewEngins()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }

        protected void HandleDevexpressErrors(object sender, EventArgs e) =>
            ASPxWebControl.SetCallbackErrorMessage(
                "امکان اجرای درخواست فراهم نشد، لطفا با تیم توسعه تماس حاصل فرمایید");
    }
}