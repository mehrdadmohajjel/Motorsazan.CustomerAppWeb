using Motorsazan.CustomerAppWeb.Shared.Models.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Motorsazan.CustomerAppWeb.Client.Api
{
    public class ApiConnector<T>
    {
        public static async Task<T> Post(string baseUrl, string method, string token = null, object parameters = null,
            int timeOutInSecond = 60 * 10)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromSeconds(timeOutInSecond);
                var hasToken = !string.IsNullOrEmpty(token);
                if(hasToken)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.PostAsJsonAsync(method, parameters);
                if(response.IsSuccessStatusCode)
                {
                    var jsonResult = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<T>(jsonResult);
                }

                if(response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new LoginException();
                }

                var httpErrorObject = response.Content.ReadAsStringAsync().Result;
                var anonymousErrorObject = new {message = "", ModelState = new Dictionary<string, string[]>()};
                var deserializedErrorObject =
                    JsonConvert.DeserializeAnonymousType(httpErrorObject, anonymousErrorObject);

                throw new AlertException(deserializedErrorObject.message);
            }
        }
    }
}