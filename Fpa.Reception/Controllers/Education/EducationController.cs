using Application.Employee;
using Application.HttpClient;
using Domain.Interface;
using lc.fitnesspro.library;
using Microsoft.AspNetCore.Mvc;
using reception.fitnesspro.ru.Misc;
using Service.lC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Service.Schedule.MySql.Model;
using Service.Schedule.MySql;

namespace reception.fitnesspro.ru.Controllers.Education
{
    [Route("[controller]")]
    [TypeFilter(typeof(ResourseLoggingFilter))]
    [TypeFilter(typeof(LoggedResultFilterAttribute))]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly IAppContext context;
        private readonly ILogger logger;

        private EmployeeHttpClient employeeHttpClient;
        private ProgramHttpClient programHttpClient;
        private readonly AssignHttpClient assignHttpClient;
        private readonly DisciplineHttpClient disciplineHttpClient;
        private readonly EducationFormHttpClient educationFormHttpClient;
        private readonly ControlTypeHttpClient controlTypeHttpClient;
        private readonly IScheduleService schedule;
        EmployeeMethods employeeAction;

        public EducationController(IAppContext context, ILoggerFactory loggerFactory,

            EmployeeHttpClient employeeHttpClient,
            ProgramHttpClient programHttpClient,
            AssignHttpClient assignHttpClient,
            DisciplineHttpClient disciplineHttpClient,
            EducationFormHttpClient educationFormHttpClient,
            ControlTypeHttpClient controlTypeHttpClient, 
            IScheduleService schedule
            )
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger(this.ToString());

            this.employeeHttpClient = employeeHttpClient;
            this.programHttpClient = programHttpClient;
            this.assignHttpClient = assignHttpClient;
            this.disciplineHttpClient = disciplineHttpClient;
            this.educationFormHttpClient = educationFormHttpClient;
            this.controlTypeHttpClient = controlTypeHttpClient;
            this.schedule = schedule;
            employeeAction = new EmployeeMethods(employeeHttpClient, assignHttpClient);
        }

        [HttpGet]
        [Route("GetProgramSiblings")]
        public async Task<ActionResult<IEnumerable<Domain.Education.Program>>> GetSiblings(Guid disciplineKey) // EducationStructureViewModel
        {
            if (disciplineKey == default)
            {
                ModelState.AddModelError(nameof(disciplineKey), "Ключ запроса не указан");
                return BadRequest(ModelState);
            }

            try
            {
                var programs = await context.Education.GetProgramsByDiscipline(disciplineKey);

                if (programs == default) return NoContent();

                return programs.ToList();
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }


        [HttpGet]
        [Route("GetGroupSchedule")]
        public async Task<ActionResult<IEnumerable<GroupEventInfo>>> GetGroupScheduleFromService(Guid groupKey)
        {
            try
            {
                var setting = await context.Setting.FindGroupSettings(groupKey);

                if (setting == default) return NoContent();

                var result = await schedule.GroupSchedule(setting.ScheduleGroupId);

                return result.ToList();
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }


        #region Old

        [HttpGet]
        [Route("Program/FindByEmployee")]
        [Obsolete]
        public async Task<ActionResult<IEnumerable<EducationInfoViewModel>>> FindByEmployee(Guid key)
        {
            if (key == default)
            {
                ModelState.AddModelError(nameof(key), "Ключ запроса не указан");
                return BadRequest(ModelState);
            }


            try
            {
                var client = new EducationProgram(new Manager("Kloder", "Kaligula2"));

                var teacherProgramKeys = (await client.GetProgramGuidByTeacher(key).ConfigureAwait(false)).ToList();

                var client2 = new EducationProgram(new Manager("Kloder", "Kaligula2"));

                var queryPrograms = (await client2.Find(teacherProgramKeys).ConfigureAwait(false)).ToList();

                var teacherProgramSource = queryPrograms.Where(x=>x.Disciplines.Any(d=>d.ControlTypeKey != default)).ToList();

                var teacherPrograms = teacherProgramSource.Select(x =>
                    new ProgramDto
                    {
                        Key = x.Key,
                        Title = x.Title,
                        EducationFormKey = x.EducationFormKey,
                        Teachers = x.Teachers.Select(x => x.TeacherKey),
                        Disciplines = x.Disciplines.Select(d => new DisciplineInfo
                        {
                            DisciplineKey = d.DisciplineKey,
                            ControlTypeKey = d.ControlTypeKey
                        })
                    }
                ).ToList();

                // get program disciplines
                var disciplineInfo = (await disciplineHttpClient.Find(teacherPrograms.SelectMany(x => x.Disciplines).Select(d => d.DisciplineKey))).ToList();

                // get teachers
                var teacherKeys = teacherPrograms.SelectMany(x => x.Teachers).Distinct().ToList();
                var teachers = (await employeeAction.GetByKeys(teacherKeys)).ToList();

                // get controltypes
                var controlTypeKeys = teacherPrograms.SelectMany(x => x.Disciplines).Select(x => x.ControlTypeKey).Where(x => x != default).Distinct().ToList();
                //var controlTypes = (await controlTypeHttpClient.GetByKeys(controlTypeKeys)).ToList();
                var controlTypes = (await context.Education.GetControlTypesByKeys(controlTypeKeys)).ToList();

                // get program education forms
                var educationFormKeys = teacherPrograms.Select(x => x.EducationFormKey).Where(e => e != default).Distinct().ToList();
                var educationForms = (await educationFormHttpClient.GetByKeys(educationFormKeys)).ToList();

                var result = teacherPrograms?.Select(x =>
                    new EducationInfoViewModel(x)
                        .AddEducation(educationForms)
                        .AddTeachers(teachers)
                        .AddDisciplines(disciplineInfo, controlTypes)
                );

                if (result == default) return NoContent();

                // get program groups
                // get group subgroup
                // get limits

                return result.ToList();
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }

        }


        [HttpGet]
        [Route("Program/FindSiblings")]
        [Obsolete]
        public async Task<ActionResult<EducationStructureViewModel>> FindProgramsWithDisciplineKey(Guid key)
        {
            if (key == default)
            {
                ModelState.AddModelError(nameof(key), "Ключ запроса не указан");
                return BadRequest(ModelState);
            }

            try
            {
                var client = new EducationProgram(new Manager("Kloder", "Kaligula2"));

                var programs = await client.FindSiblings(key);
                if (programs == null || programs.Any() == false) return NoContent();

                var groups = await client.FindProgramGroup(programs.Select(x => x.Key));

                var subGroups = await client.FindSubgroups(groups.Select(x => x.Key));

                var viewModel = new EducationStructureViewModel(programs, groups, subGroups);

                if (viewModel == default) return NoContent();

                return viewModel;
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }

        }

        #endregion
    }
}
