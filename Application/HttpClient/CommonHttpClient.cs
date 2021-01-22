using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
