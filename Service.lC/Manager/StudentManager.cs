using Service.lC.Model;
using Service.lC.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.lC.Manager
{
    public class StudentManager
    {
        private readonly ContractProvider contractProvider;

        public StudentManager(
            ContractProvider contractProvider
            )
        {
            this.contractProvider = contractProvider;
        }

        public async Task IncludeContracts(IEnumerable<Student> students)
        {
            var studentKeys = students.Select(x => x.Key).ToList().Distinct();
            var contracts = await contractProvider.FilterByStudent(studentKeys);

            students.ToList()
                .ForEach(x => x.Contract = contracts.Where(c=>c.Students.Any(k=>k == x.Key)));
        }

    }
}
