using System.Web.Mvc;

namespace Motorsazan.CustomerAppWeb.Client.Controllers
{
    public class HomeController: BaseController
    {
        public ActionResult Index()
        {
            var token = Request.QueryString.Get(JwtKeyName);
            if(IsCookieContainsJwt() && string.IsNullOrEmpty(token))
            {
                SetUserInfoInViewBag();
                return View();
            }

            if(string.IsNullOrWhiteSpace(token))
            {
                ForceUserToLogin();
            }

            SetSessionAndCookie(token);
            SetUserInfoInViewBag();

            return RedirectToAction("Index");
        }

        [Route("~/Forbidden")]
        public ActionResult Forbidden() => View("Forbidden");
    }
}