﻿using System.Net.Http;

namespace Service.lC
{
    public class ProgramClient
    {
        private readonly HttpClient httpClient;
        private string rootUrl = "Program/";

        public ProgramClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        //public async IEnumerable<Program> GetAll()
        //{
        //    var query = await httpClient.GetAsync(rootUrl + "");

        //    return query;
        //}
    }
}
