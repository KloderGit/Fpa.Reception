using lc.fitnesspro.library.Interface;
using Service.lC.Model;
using Service.lC.Provider;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.lC.Manager
{
    public class ProgramManager
    {
        private readonly ProviderDepository depository;
        private readonly IManager lcManager;

        public ProgramManager(ProviderDepository depository)
        {
            this.depository = depository;
        }

        public async Task<IEnumerable<Program>> FilterByTeacher(Guid teacherKey)
        {
            var programs = await depository.Program.FilterByTeacher(teacherKey);

            return programs;
        }



    }
}
