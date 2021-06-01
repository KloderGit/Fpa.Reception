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
        private readonly StudentProvider studentProvider;

        public StudentManager(
            ContractProvider contractProvider,
            StudentProvider studentProvider
            )
        {
            this.contractProvider = contractProvider;
            this.studentProvider = studentProvider;
        }

        public async Task<IEnumerable<Student>> GetStudentsByKeys(IEnumerable<Guid> studentKeys)
        {
            if(studentKeys == default || studentKeys.Count() ==0) return Enumerable.Empty<Student>();

            var students = await studentProvider.Repository.GetAsync(studentKeys);

            return students;
        }


        public async Task IncludeContracts(IEnumerable<Student> students)
        {
            if(students != default && students.Count() > 0)
            {
            var studentKeys = students.Select(x => x.Key).ToList().Distinct();
            var contracts = await contractProvider.FilterByStudent(studentKeys);

            students.ToList()
                .ForEach(x => x.Contract = contracts.Where(c=>c.Students.Any(k=>k == x.Key)));
            }
        }

    }
}
