using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Application.HttpClient
{
    public class IdentityHttpClient : CommonHttpClient
    {
        public IdentityHttpClient(System.Net.Http.HttpClient client)
            : base(client: client)
        {}

        public async Task<UserInfo> GetUserInfo(string token)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await Client.GetAsync("/connect/userinfo");

            UserInfo user = null;

            if (request.IsSuccessStatusCode)
            {
                var json = await request.Content.ReadAsStringAsync();

                user = JsonConvert.DeserializeObject<UserInfo>(json);
            }

            return user;
        }
    }
}
