using System.Net.Http.Headers;

namespace Application.HttpClient
{
    public abstract class CommonHttpClient
    {
        public System.Net.Http.HttpClient Client { get; }

        public CommonHttpClient(System.Net.Http.HttpClient client)
        {
            Client = client;
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
