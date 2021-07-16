using Newtonsoft.Json;
using Service.lC.Dto;
using Service.lC.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Service.lC.Repository
{
    public class AttestationTableRepository : GenericRepository<AttestationTable, AttestationTableDto>
    {
        public AttestationTableRepository(BaseHttpClient httpClient, string endpoint)
            : base(httpClient, endpoint)
        { }

        public async Task<AttestationTable> Create(AttestationTable table)
        {
            var objToJson = JsonConvert.SerializeObject(table, serializerSettings);
            var stringContent = new StringContent(objToJson, Encoding.UTF8, "application/json");

            var request = await http.Client.PostAsync(endpoint + "/" + "Create", stringContent).ConfigureAwait(false);
            request.EnsureSuccessStatusCode();

            var response = await request.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<AttestationTable>(response);

            return result;
        }
    }
}
