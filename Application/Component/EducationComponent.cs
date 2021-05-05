using Domain;
using Domain.Interface;
using Domain.Model.Education;
using Mapster;
using Service.lC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lcService = Service.lC;

namespace Application.Component
{
    public class EducationComponent : IEducationComponent
    {
        private readonly Context lcService;

        public EducationComponent(Context lcService)
        {
            this.lcService = lcService;
        }

        public async Task<IEnumerable<Domain.Education.Program>> GetProgramsByDiscipline(Guid disciplineKey)
        { 
            var programManager = lcService.Program;

            var programs = await programManager.FilterByDiscipline(disciplineKey);
                await programManager.IncludeTeachers(programs);
                await programManager.IncludeGroups(programs);

            var groupManager = lcService.Group;
            var groups = programs.SelectMany(x => x.Groups);
                await groupManager.IncludeSubGroups(groups);

            var domain = programs.Adapt<IEnumerable<Domain.Education.Program>>();

            return domain;
        }

        public async Task<IEnumerable<Domain.Education.Program>> GetProgramsByKeys(IEnumerable<Guid> programKeys)
        {
            var programs = await lcService.Program.GetPrograms(programKeys);

            var result = programs.Adapt<IEnumerable<Domain.Education.Program>>();

            return result;
        }

        public async Task<IEnumerable<BaseInfo>> GetDisciplinesByKeys(IEnumerable<Guid> disciplineKeys)
        {
            var disciplines = await lcService.Education.GetDisciplinesByKeys(disciplineKeys);

            return disciplines.Adapt<IEnumerable<BaseInfo>>();
        }



        public async Task<Domain.Education.Program> GetStudentEducation(Guid programKey)
        {
            var foundedProgramQuery = await lcService.Program.GetProgram(programKey);
            await lcService.Program.IncludeDisciplines(new List<Service.lC.Model.Program>() { foundedProgramQuery });

            var domain = foundedProgramQuery.Adapt<Domain.Education.Program>();

            return domain;
        }

        public async Task<IEnumerable<ControlType>> GetControlTypesByKeys(IEnumerable<Guid> controlTypeKeys)
        { 
            var controlTypes = await lcService.ControlType.GetControlTypesByKeys(controlTypeKeys);
            //var castedControlTypes = controlTypes.Select(x=> new Service.lC.Model.ControlType{ Key = x.Key, Title = x.Title });
                await lcService.ControlType.IncludeScoreType(controlTypes);

            var scoreTypes = controlTypes.SelectMany(x=>x.RateType.Select(s=>s.RateKey));
                await lcService.ControlType.IncludeRateType(scoreTypes);

            var domain = controlTypes.Adapt<IEnumerable<ControlType>>();

            return domain;
        }

        public async Task<IEnumerable<BaseInfo>> GetRates()
        {
            var dto = await lcService.ControlType.GetAllRates();

            var model = dto.Adapt<IEnumerable<BaseInfo>>();

            return model;
        }

        public async Task<IEnumerable<BaseInfo>> GetTeachers(IEnumerable<Guid> teacherKeys)
        {
            var dto = await lcService.Education.GetTeachersByKeys(teacherKeys);

            var model = dto.Adapt<IEnumerable<BaseInfo>>();

            return model;
        }
    }
}
