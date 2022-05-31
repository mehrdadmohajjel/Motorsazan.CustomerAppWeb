using Motorsazan.CustomerAppWeb.Shared.AuthService;
using Motorsazan.CustomerAppWeb.Shared.Models.Base;
using Motorsazan.CustomerAppWeb.Shared.Models.DomainModels;
using Motorsazan.CustomerAppWeb.Shared.Utilities;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace Motorsazan.CustomerAppWeb.Client.Controllers
{
    public class BaseController: Controller
    {
        protected const string JwtKeyName = "MotorsazanJsonWebToken";

        private const string BaseLoginDomain = @"http://erp-server/";

        private void ClearCookie()
        {
            // To clear cookie from Browser for sign out
            Response.Cookies.Remove(JwtKeyName);
            var cookie = new HttpCookie(JwtKeyName) {Expires = DateTime.Now.AddDays(-1)};
            Response.Cookies.Add(cookie);

            // To clear cookie from request in order to change old token 
            // in cookie with new token when old token is also valid
            Request.Cookies.Clear();
        }

        private void ClearSessionInfo() => Session.Clear();

        public ActionResult ConvertBase64ToFile(string base64, string fileName)
        {
            var fileBytes = Convert.FromBase64String(base64);
            return File(fileBytes, MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public async Task<ActionResult> DoLogin(string userName, string password)
        {
            if(!ModelState.IsValid)
            {
                throw new AlertException("اطلاعات هویتی به درستی وارد نشده است");
            }

            using(var client = new HttpClient())
            {
                dynamic apiInputParameters = new {userName, password};
                var paramsAsJson = JsonConvert.SerializeObject(apiInputParameters);
                var data = new StringContent(paramsAsJson, Encoding.UTF8, "application/json");

                const string url = BaseLoginDomain + "Home/GetJsonTokenByUserNameAndPassword";
                var response = await client.PostAsync(url, data);
                var userJsonWebToken = response.Content.ReadAsStringAsync().Result;

                if(response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }

                if(string.IsNullOrEmpty(userJsonWebToken))
                {
                    throw new AlertException("امکان اجرای درخواست فراهم نشد، لطفا با تیم پشتیبانی تماس حاضل فرمایید");
                }

                SetSessionAndCookie(userJsonWebToken);
                SetUserInfoInViewBag();
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        protected void ForceUserToLogin()
        {
            ClearCookie();

            var isPostBack = !Request.IsAjaxRequest();
            if(isPostBack)
            {
                Response.Redirect(BaseLoginDomain);
            }

            throw new LoginException();
        }

        protected Tbl_User GetCurrentUser()
        {
            var token = GetUserToken();
            return GetUerInfoByJwt(token);
        }

        protected object GetDataFromCache(string cacheKey) => HttpContext.Cache.Get(cacheKey);

        public string GetUserToken()
        {
            var isCookieContainsJwt = IsCookieContainsJwt();
            var userToken = isCookieContainsJwt
                ? Request.Cookies.Get(JwtKeyName)?.Value
                : (string)Session[JwtKeyName];
            return userToken;
        }

        private Tbl_User GetUerInfoByJwt(string userToken)
        {
            const string cacheKey = "UserInfo";
            var cachedInfo = Session[cacheKey];
            if(cachedInfo != null)
            {
                return (Tbl_User)cachedInfo;
            }

            var input = new WebServiceInputGetEndUserInfoByJsonWebToken
            {
                Parameters = new WebServiceInputGetEndUserInfoByJsonWebTokenParams {JsonWebToken = userToken}
            };
            var client = new AppClient();
            var output = client.GetUserInfoByJsonWebToken(input);
            client.Close();

            if(output.Status.Code != "G00000")
            {
                ForceUserToLogin();
                return null;
            }

            if(output.Parameters != null)
            {
                var userInfo = new Tbl_User
                {
                    FirstName = output.Parameters.FirstName,
                    LastName = output.Parameters.LastName,
                    UserID = output.Parameters.UserID,
                    EID = output.Parameters.EID
                };
                Session.Add(cacheKey, userInfo);
                return userInfo;
            }

            ForceUserToLogin();
            return null;
        }

        protected bool IsSessionContainsJwt() => Session[JwtKeyName] != null;

        protected bool IsCookieContainsJwt() => Request.Cookies.Get(JwtKeyName) != null;

        public ActionResult NavigationBar()
        {
            const string partial = "~/Views/Shared/Partials/_NavigationBar.cshtml";
            return PartialView(partial);
        }

        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var controllerName = context.RouteData.Values["controller"].ToString();
            var actionName = context.RouteData.Values["action"].ToString();
            if(string.Equals(actionName, "DoLogin", StringComparison.CurrentCultureIgnoreCase) ||
               string.Equals(actionName, "SignOut", StringComparison.CurrentCultureIgnoreCase) ||
               (string.Equals(actionName, "Index", StringComparison.CurrentCultureIgnoreCase) &&
                string.Equals(controllerName, "Home", StringComparison.CurrentCultureIgnoreCase)))
            {
                return;
            }

            var shouldForceToLogin = !IsCookieContainsJwt();
            if(shouldForceToLogin)
            {
                ForceUserToLogin();
                return;
            }

            var userJsonWebToken = GetUserToken();

            var isJwtValid = UserTools.IsSessionValidByJsonWebToken(userJsonWebToken);
            if(!isJwtValid)
            {
                ForceUserToLogin();
                return;
            }

            var userInfo = GetUerInfoByJwt(userJsonWebToken);
            if(userInfo != null)
            {
                SetUserInfoInViewBag(userInfo);
            }
            else
            {
                ForceUserToLogin();
            }
        }

        protected void SetDataInCache(string cacheKey, dynamic cacheData, int cacheSeconds = 30)
        {
            var expirationTime = DateTime.UtcNow.AddSeconds(cacheSeconds);
            var slidingExpire = Cache.NoSlidingExpiration;
            HttpContext.Cache.Insert(cacheKey, cacheData, null, expirationTime, slidingExpire);
        }

        protected void SetUserInfoInViewBag(Tbl_User userInfo = null)
        {
            if(userInfo == null)
            {
                userInfo = GetCurrentUser();
            }

            var fullName = $"{userInfo.FirstName} {userInfo.LastName}";

            Session["EID"] = userInfo.EID;
            Session["Username"] = fullName;
            Session["OnlineUserId"] = userInfo.UserID;

            ViewBag.UserFullName = fullName;
            ViewBag.EID = userInfo.EID;
            ViewBag.UserId = userInfo.UserID;
            ViewBag.JWT = GetUserToken();
            ViewBag.CanSaveToken = true;

            var avatarBaseUrl = "http://erp-server/Images/PersonalPic/";
            ViewBag.UserAvatarUrl = avatarBaseUrl + userInfo.EID + ".jpg";
        }

        protected void SetSessionAndCookie(string userJsonWebToken)
        {
            ClearSessionInfo();
            ClearCookie();

            Session[JwtKeyName] = userJsonWebToken;

            var jwtCookie = new HttpCookie(JwtKeyName, userJsonWebToken) {Expires = DateTime.Now.AddHours(12)};

            if(IsCookieContainsJwt())
            {
                Response.Cookies.Set(jwtCookie);
                return;
            }

            Response.Cookies.Add(jwtCookie);
        }

        public ActionResult ShortcutBar()
        {
            const string partial = "~/Views/Shared/Partials/_ShortcutBar.cshtml";
            return PartialView(partial);
        }

        [HttpPost]
        public string SignOut()
        {
            ClearSessionInfo();
            ClearCookie();
            return BaseLoginDomain;
        }
    }
}