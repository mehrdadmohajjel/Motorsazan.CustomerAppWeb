using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace Motorsazan.CustomerAppWeb.Shared.Utilities
{
    public class WebApiConsumer<T>
    {
        private readonly HttpClient _client;

        public WebApiConsumer(string baseUrl)
        {
            BaseURL = baseUrl;
            _client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private string BaseURL { get; }

        public async Task<T> Get(string @params)
        {
            var responseMessage = await _client.GetAsync(BaseURL + "?" + @params).ConfigureAwait(false);
            var result = (T) Activator.CreateInstance(typeof(T), new object[] { });

            if (!responseMessage.IsSuccessStatusCode) return result;

            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseData);
        }

        [HttpPost]
        public async Task<bool> Add(T item)
        {
            var responseMessage = await _client.PostAsJsonAsync(BaseURL, item);
            return responseMessage.IsSuccessStatusCode;
        }

        [HttpPut]
        public async Task<bool> Edit(int id, T item)
        {
            var responseMessage = await _client.PutAsJsonAsync(BaseURL + "/" + id, item);
            return responseMessage.IsSuccessStatusCode;
        }

        [HttpDelete]
        public async Task<bool> Delete(int id)
        {
            var responseMessage = await _client.DeleteAsync(BaseURL + "/" + id);
            return responseMessage.IsSuccessStatusCode;
        }
    }
}