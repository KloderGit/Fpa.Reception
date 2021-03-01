using System.Net.Http;
using System.Net.Http.Headers;

namespace Service.lC
{
    public class BaseHttpClient
    {
        public HttpClient Client { get; }

        public BaseHttpClient(HttpClient client)
        {
            Client = client;
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
