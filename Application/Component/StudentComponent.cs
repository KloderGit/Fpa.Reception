using Domain.Interface;
using Mapster;
using Service.lC;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Component
{
    public class StudentComponent : IStudentComponent
    {
        private readonly Context lcservice;

        public StudentComponent(Context lcservice)
        {
            this.lcservice = lcservice;
        }

        public async Task<Domain.Education.Program> GetEducationByContract(Guid key)
        {
            var contractManager = lcservice.Contract;
            var contract = await contractManager.Get(key);

            var programKey = contract.EducationProgram.Key;

            var programManager = lcservice.Program;

            var program = await programManager.GetProgram(programKey);
                await programManager.IncludeDisciplines(new List<Service.lC.Model.Program> { program });
                await programManager.IncludeEducationForm(new List<Service.lC.Model.Program> { program });

            var domain = program.Adapt<Domain.Education.Program>();

            return domain;
        }
    }
}
