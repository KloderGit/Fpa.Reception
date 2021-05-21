using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Service.lC
{
    public class BaseHttpClient
    {
        public HttpClient Client { get; }

        public BaseHttpClient(HttpClient client, IHttpContextAccessor contextAccessor)
        {
            Client = client;
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (contextAccessor != null && contextAccessor.HttpContext.Request.Headers.Any(x => x.Key == "Aggregate-Request-Id"))
                    client.DefaultRequestHeaders.Add("Aggregate-Request-Id", contextAccessor.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Aggregate-Request-Id").Value.ToString());
        }
    }
}
