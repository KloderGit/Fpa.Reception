using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Component;
using Application.Employee;
using Application.HttpClient;
using Application.Program;
using Domain.Interface;
using lc.fitnesspro.library;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reception.fitnesspro.ru.Controllers.Education.ViewModel;
using reception.fitnesspro.ru.Controllers.Teacher;
using Service.lC;
using Service.lC.Provider;
using Service.MongoDB;

namespace reception.fitnesspro.ru.Controllers.Education
{
    [Route("[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly IAppContext context;

        private EmployeeHttpClient employeeHttpClient;
        private ProgramHttpClient programHttpClient;
        private readonly AssignHttpClient assignHttpClient;
        private readonly DisciplineHttpClient disciplineHttpClient;
        private readonly EducationFormHttpClient educationFormHttpClient;
        private readonly ControlTypeHttpClient controlTypeHttpClient;

        ProgramMethods programAction;
        EmployeeMethods employeeAction;

        public EducationController(
            IAppContext context,
            EmployeeHttpClient employeeHttpClient,
            ProgramHttpClient programHttpClient,
            AssignHttpClient assignHttpClient,
            DisciplineHttpClient disciplineHttpClient,
            EducationFormHttpClient educationFormHttpClient,
            ControlTypeHttpClient controlTypeHttpClient)
        {
            this.context = context;
            this.employeeHttpClient = employeeHttpClient;
            this.programHttpClient = programHttpClient;
            this.assignHttpClient = assignHttpClient;
            this.disciplineHttpClient = disciplineHttpClient;
            this.educationFormHttpClient = educationFormHttpClient;
            this.controlTypeHttpClient = controlTypeHttpClient;

            programAction = new ProgramMethods(programHttpClient);
            employeeAction = new EmployeeMethods(employeeHttpClient, assignHttpClient);
        }

        [HttpGet]
        [Route("Program/FindByEmployee")]
        public async Task<ActionResult<IEnumerable<EducationInfoViewModel>>> FindByEmployee(Guid key)
        {
            var client = new EducationProgram(new Manager("Kloder", "Kaligula2"));

            if (key == default)
            {
                ModelState.AddModelError(nameof(key), "Ключ запроса не указан");
                return BadRequest(ModelState);
            }

            //var prgs = await context.Teacher.GetEducation(key).ConfigureAwait(false);
            //prgs.ToList();

            // get programs with teacher
            var teacherProgramKeys = await client.GetProgramGuidByTeacher(key).ConfigureAwait(false);

            var client2 = new EducationProgram(new Manager("Kloder", "Kaligula2"));

            var teacherProgramSource = await client2.Find(teacherProgramKeys).ConfigureAwait(false);

            var teacherPrograms = teacherProgramSource.Select(x =>
                new ProgramDto
                {
                    Key = x.Key,
                    Title = x.Title,
                    EducationFormKey = x.EducationFormKey,
                    Teachers = x.Teachers.Select(x=>x.TeacherKey),
                    Disciplines = x.Disciplines.Select(d => new DisciplineInfo
                    {
                        DisciplineKey = d.DisciplineKey,
                        ControlTypeKey = d.ControlTypeKey
                    })
                }
            );

            // get program disciplines
            var disciplineInfo = await disciplineHttpClient.Find(teacherPrograms.SelectMany(x => x.Disciplines).Select(d=>d.DisciplineKey));

            // get teachers
            var teacherKeys = teacherPrograms.SelectMany(x => x.Teachers);
            var teachers = await employeeAction.GetByKeys(teacherKeys);

            // get controltypes
            var controlTypeKeys = teacherPrograms.SelectMany(x => x.Disciplines).Select(x => x.ControlTypeKey).Where(x => x != default);
            var controlTypes = await controlTypeHttpClient.GetByKeys(controlTypeKeys);

            // get program education forms
            var educationFormKeys = teacherPrograms.Select(x => x.EducationFormKey).Where(e => e != default);
            var educationForms = await educationFormHttpClient.GetByKeys(educationFormKeys);


            var result = teacherPrograms?.Select(x=>
                new EducationInfoViewModel(x)
                    .AddEducation(educationForms)
                    .AddTeachers(teachers)
                    .AddDisciplines(disciplineInfo, controlTypes)
            );


            // get program groups
            // get group subgroup
            // get limits

            return result.ToList();
        }

        [HttpGet]
        [Route("Program/FindSiblings")]
        public async Task<ActionResult<EducationStructureViewModel>> FindProgramsWithDisciplineKey(Guid key)
        {
            var client = new EducationProgram(new Manager("Kloder", "Kaligula2"));

            var programs = await client.FindSiblings(key);
            if (programs == null || programs.Any() == false) return NoContent();

            var groups = await client.FindProgramGroup(programs.Select(x => x.Key));

            var subGroups = await client.FindSubgroups(groups.Select(x=>x.Key));

            var viewModel = new EducationStructureViewModel(programs, groups, subGroups);

            return viewModel;
        }

        [HttpGet]
        [Route("Program/GetSiblings")]
        public async Task<ActionResult<dynamic>> GetSiblings(Guid key)
        {
            var programs = await context.Education.GetProgramsByDiscipline(key);

            return programs.ToList();
        }
    }
}
