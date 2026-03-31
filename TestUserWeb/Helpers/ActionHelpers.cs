using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TestCrudWeb.Helpers
{
    public interface IActionHelpers
    {
        Task<T> ExcecuteAsync<T>(string url, string method, object content = null);        
    }

    public class ActionHelpers : IActionHelpers
    {
        private readonly HttpClient _http;
        public ActionHelpers(
            HttpClient http
        )
        {
            _http = http;
        }       

        public async Task<T> ExcecuteAsync<T>(string url, string method, object content = null)
        {
            var requestMessage = new HttpRequestMessage()
            {
                Method = new HttpMethod(method),
                RequestUri = new Uri(url),
                Content = JsonContent.Create(content)
            };
            requestMessage.Content.Headers.TryAddWithoutValidation("x-custom-header", "value");
            var response = await _http.SendAsync(requestMessage);
            return await response.Content.ReadFromJsonAsync<T>();

        }
    }
}