using System.Net.Http.Headers;

namespace Service.lC
{
    public abstract class RestProvider
    {
        public System.Net.Http.HttpClient Client { get; }

        public RestProvider(System.Net.Http.HttpClient client)
        {
            Client = client;
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

    }
}
