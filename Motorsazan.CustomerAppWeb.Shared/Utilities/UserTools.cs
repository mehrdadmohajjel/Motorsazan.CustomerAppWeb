using System;
using System.Linq;
using System.Web;
using Motorsazan.CustomerAppWeb.Shared.AuthService;
using Motorsazan.CustomerAppWeb.Shared.Models.ApiModels.Output;

namespace Motorsazan.CustomerAppWeb.Shared.Utilities
{
    public static class UserTools
    {
        public static int? GetOnlineUser()
        {
            var sessionKey = "OnlineUserId";
            var currentSessionValue = HttpContext.Current.Session[sessionKey];
            return currentSessionValue != null ? Convert.ToInt32(currentSessionValue) : 0;
        }

        public static int GetUserIdByJsonWebToken(string jsonWebToken)
        {
            try
            {
                var input = new WebServiceInputGetEndUserInfoByJsonWebToken
                {
                    Parameters = new WebServiceInputGetEndUserInfoByJsonWebTokenParams
                    {
                        JsonWebToken = jsonWebToken
                    }
                };
                var client = new AppClient();
                var output = client.GetUserInfoByJsonWebToken(input);
                client.Close();

                if (output.Status.Code != "G00000") return 0;

                return output.Parameters?.UserID ?? 0;
            }
            catch
            {
                return 0;
            }
        }

        public static User GetUserName(long userId)
        {
            try
            {
                var baseURL = "http://erp-server:2021/api/GetUserInfo";
                var server = new WebApiConsumer<UserInfo>(baseURL);
                var response = server.Get("UserId=" + userId);
                var successStatus = "1";

                var isResponseValid = !Equals(response, null) && response.Result.Status.Code == successStatus;

                if (!isResponseValid) return null;

                var userInfo = response.Result.Params.List;
                var isUserInfoValid = userInfo != null && userInfo.Any();

                return isUserInfoValid ? userInfo[0] : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool HasAccessToEvent(int? userId, string formCode, string eventCode, string systemCode)
        {
            try
            {
                var baseUrl = "http://erp-server/api/CheckEventAccess";
                var server = new WebApiConsumer<CheckEventAccess>(baseUrl);
                var url = $"UserID={userId}&FormCode={formCode}&SystemCode={systemCode}&EventCode={eventCode}";
                var response = server.Get(url);
                var isResponseValid = response.Result.Status.Code == "G00000";
                if (!isResponseValid) return false;

                var result = response.Result.Params.HasAccessToPage;
                return result.HasAccess;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool HasAccessToForm(int? userId, string formCode, string systemCode)
        {
            try
            {
                const string baseUrl = "http://erp-server/api/CheckFormAccess";
                var server = new WebApiConsumer<CheckFormAccess>(baseUrl);
                var url = $"UserID={userId}&FormCode={formCode}&SystemCode={systemCode}";
                var response = server.Get(url);
                var isResponseValid = !Equals(response, null) && response.Result.Status.Code == "G00000";

                if (!isResponseValid) return false;

                var result = response.Result.Params.HasAccessToPage;
                return result?.HasAccess ?? false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsSessionValidByJsonWebToken(string jsonWebToken)
        {
            try
            {
                var input = new WebServiceInputValidateSessionByJsonWebToken
                {
                    Parameters = new WebServiceInputValidateSessionByJsonWebTokenParams
                    {
                        JsonWebToken = jsonWebToken
                    }
                };
                var client = new AppClient();
                var output = client.ValidateSessionByJsonWebToken(input);
                client.Close();

                return output.Status.Code == "G00000" && output.Parameters.IsValid;
            }
            catch
            {
                return false;
            }
        }
    }
}