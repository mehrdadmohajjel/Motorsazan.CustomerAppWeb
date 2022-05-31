using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Motorsazan.CustomerAppWeb.Api.HttpActionResults
{
    public class ErrorContentResult: IHttpActionResult
    {
        private readonly string _content;
        private readonly string _mediaType;
        private readonly HttpRequestMessage _request;

        public ErrorContentResult(string content, string mediaType, HttpRequestMessage request)
        {
            _content = content;
            _request = request;
            _mediaType = mediaType;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(_content, Encoding.UTF8, _mediaType), RequestMessage = _request
            });
    }
}