using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> GetAsync(this HttpClient client, string url, object content)
        {
            var json = JsonSerializer.Serialize(content);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await client.SendAsync(request).ConfigureAwait(false);

            return result;
        }
    }
}
