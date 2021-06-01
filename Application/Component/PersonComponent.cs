using Application.Mappings;
using Domain;
using Domain.Interface;
using Domain.Education;
using Mapster;
using Service.lC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Component
{
    public class PersonComponent : IPersonComponent
    {
        private readonly Context lcService;

        public PersonComponent(Context lcService)
        {
            this.lcService = lcService;
        }

        public async Task<IEnumerable<Domain.Education.Person>> GetInfo(IEnumerable<Guid> personKeys)
        {
            var personsPipe = lcService.Person;
            var studentsPipe = lcService.Student;
            var contractPipe = lcService.Contract;
            var programPipe = lcService.Program;

            var persons = await personsPipe.Get(personKeys);
            await personsPipe.IncludeStudents(persons);
            await studentsPipe.IncludeContracts(persons.SelectMany(x => x.Students));

            var contracts = persons.SelectMany(p => p.Students.SelectMany(s => s.Contract));
            await contractPipe.IncludePrograms(contracts);
            await contractPipe.IncludeGroup(contracts);
            await contractPipe.IncludeSubGroup(contracts);

            var programs = contracts.Select(x => x.EducationProgram).Cast<Service.lC.Model.Program>().ToList();
            await programPipe.IncludeEducationForm(programs);

            var domen = persons.Adapt<IEnumerable<Domain.Education.Person>>().ToList();

            domen.ForEach(d =>
                d.Students.ToList().ForEach(
                    s => s.Educations.ToList().ForEach(
                        e => e.EducationForm = (programs.FirstOrDefault(p => p.Key == e.EducationProgram.Key).EducationForm).Adapt<BaseInfo>())));


            return domen;

        }

        public async Task<IEnumerable<Domain.Education.Person>> GetByStudent(IEnumerable<Guid> studentsKeys)
        {
            var dto = await lcService.Person.FindByStudents(studentsKeys);

            var result = dto.Adapt<IEnumerable<Domain.Education.Person>>();

            return result;
        }

        public async Task<IEnumerable<Domain.Education.Person>> FindByQuery(string queryString)
        {
            var personsQuery = await lcService.Person.FindByQuery(queryString);
            var persons = personsQuery.ToList();

            await lcService.Person.IncludeStudents(persons);
            await lcService.Student.IncludeContracts(persons.ToList().SelectMany(x => x.Students));

            var contracts = persons.SelectMany(p => p.Students.SelectMany(s => s.Contract));
            await lcService.Contract.IncludePrograms(contracts);
            await lcService.Contract.IncludeGroup(contracts);
            await lcService.Contract.IncludeSubGroup(contracts);

            var programs = contracts.Select(x => x.EducationProgram).Cast<Service.lC.Model.Program>().ToList();
            await lcService.Program.IncludeEducationForm(programs);

            var domen = persons.Adapt<IEnumerable<Domain.Education.Person>>().ToList();

            domen.ForEach(d =>
                d.Students.ToList().ForEach(
                    s => s.Educations.ToList().ForEach(
                        e => e.EducationForm = (programs.FirstOrDefault(p => p.Key == e.EducationProgram.Key).EducationForm).Adapt<BaseInfo>())));

            return domen;
        }
    }
}
