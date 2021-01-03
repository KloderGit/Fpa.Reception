using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service.lC
{
    public class Program
    {
        private readonly HttpClient http;

        public Program(HttpClient client)
        {
            http = client;
        }

        public async Task<dynamic> GetAllEvent()
        { 
            var array = await http.GetAsync("from 1c service").ConfigureAwait(false);

            return array;
        }

        public async Task<dynamic> GetByEmployer()
        {
            var array = await http.GetAsync("from 1c service").ConfigureAwait(false);

            return array;
        }

        public async Task<dynamic> GetByStudent()
        {
            var array = await http.GetAsync("from 1c service").ConfigureAwait(false);

            return array;
        }

        public async Task<dynamic> GetByKey(Guid key)
        {
            var array = await http.GetAsync("from 1c service").ConfigureAwait(false);

            return array;
        }
    }
}
