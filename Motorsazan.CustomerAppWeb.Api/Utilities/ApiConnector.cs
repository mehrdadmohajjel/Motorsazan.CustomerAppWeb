using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Motorsazan.CustomerAppWeb.Api.Utilities
{
    public class ApiConnector<T>
    {
        public static async Task<T> Post(string baseUrl, string method, string token = null, object parameters = null)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
                    throw new Exception("Error");
                }

                //throw new AlertException(response.RequestMessage.ToString());
                throw new Exception("Error");
            }
        }
    }
}