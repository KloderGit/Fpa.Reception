using lc.fitnesspro.library.Interface;
using Microsoft.Extensions.Configuration;
using Service.lC.Dto;
using Service.lC.Manager;
using Service.lC.Model;
using Service.lC.Provider;
using Service.lC.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.lC
{
    public class Context
    {
        private readonly BaseHttpClient client;
        private readonly IManager lcManager;

        private readonly ProgramManager program;

        public Context(
            BaseHttpClient client, 
            IManager lcManager, 
            IConfiguration configuration)
        {
            this.client = client;
            this.lcManager = lcManager;
        }

        //public ProgramManager Program
        //{ 
        //    get {
        //        var repository = new GenericRepository<Program, ProgramDto>(client, "lc/Program");
        //        var provider = new ProgramProvider();
        //        program = new ProgramManager(provider);
        //    }
        //};

    }


}
