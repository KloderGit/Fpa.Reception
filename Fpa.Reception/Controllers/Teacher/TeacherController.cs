using Application.Extensions;
using Domain;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using reception.fitnesspro.ru.Controllers.Teacher.ViewModel;
using reception.fitnesspro.ru.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Teacher
{
    [ApiController]
    [Route("[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly IAppContext context;
        private readonly ILogger logger;

        public TeacherController(IAppContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger(this.ToString());
        }

        /// <summary>
        /// Get information in which programs and disciplines the teacher is invoved in
        /// </summary>
        /// <param name="key">Method takes a key of employee</param>
        /// <returns></returns>
        [HttpGet]
        [TypeFilter(typeof(ResourseLoggingFilter))]
        [Route("GetEducation")]
        public async Task<ActionResult<IEnumerable<Domain.Education.Program>>> GetEducationByEmployeeKey(Guid key)
        {
            logger.LogInformation("Получен запрос на получение программ обучения по ключу - {Key} преподавателя", key);

            if (key == default)
            {
                ModelState.AddModelError(nameof(key), "Ключ запроса не указан");
                logger.LogWarning("Ключ запроса не указан {@Error}", ModelState);
                return BadRequest(ModelState);
            }

            var programs = await context.Teacher.GetEducation(key);

            logger.LogInformation("Получено {Count} записей типа - {Type}", programs.Count(), typeof(Program).Name);
            logger.LogDebug("Полученные данные типа {Type} - {@Result}", programs.GetType().Name, programs);

            if (programs.IsNullOrEmpty()) return NoContent();

            return programs.ToList();
        }

        [HttpGet]
        [Route("GetSchedule")]
        public async Task<ActionResult<IEnumerable<Domain.Reception>>> GetScheduleFromReceptions(Guid employeeKey,
            Guid disciplineKey, DateTime fromDate, DateTime toDate)
        {
            logger.LogInformation("Получен запрос на получение рецепций для преподавателя - {EmployeeKey}, " +
                "дисциплины - {DiscoplineKey} " +
                "с фильтром по датам с {StartFilterDate} по {EndFilterDate}",
                employeeKey, disciplineKey, fromDate.ToShortDateString(), toDate.ToShortDateString());

            if (employeeKey == default) ModelState.AddModelError(nameof(employeeKey), "Ключ преподавателя не указан");
            if (disciplineKey == default) ModelState.AddModelError(nameof(disciplineKey), "Ключ дисциплины не указан");

            if (ModelState.IsValid == false)
            {
                logger.LogWarning("Данные не указанны {@Error}", ModelState);
                return BadRequest(ModelState);
            }
            
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            if(fromDate == default) fromDate = new DateTime(currentYear, currentMonth, 1);
            if(toDate == default) toDate = new DateTime(currentYear, currentMonth, DateTime.DaysInMonth(currentYear,currentMonth));
            if(toDate < fromDate)
            { 
                var temp = toDate;
                toDate = fromDate;
                fromDate = temp;
            }

            var receptions = await context.Teacher.GetReceptions(employeeKey, disciplineKey, fromDate, toDate);

            logger.LogInformation("Получено {Count} записей типа - {Type}", receptions.Count(), typeof(Domain.Reception).Name);
            logger.LogDebug("Полученные данные типа {Type} - {@Result}", receptions.GetType().Name, receptions);

            if (receptions.IsNullOrEmpty()) return NoContent();

            return receptions.ToList();
        }


        [HttpGet]
        [Route("GetTable")]
        public async Task<ActionResult<TableViewModel>> GetTableFromReception([FromQuery] Guid key)
        {
            logger.LogInformation("Получен запрос на получение рецепции-табеля по ключу - {Key} рецепции", key);

            if (key == default)
            {
                ModelState.AddModelError(nameof(key), "Ключ запроса не указан");
                logger.LogWarning("Ключ запроса не указан {@Error}", ModelState);
                return BadRequest(ModelState);
            }

            var reception = context.Reception.GetByKey(key);

            if (reception == default) return BadRequest(nameof(key));

            var disciplineKeys = reception.PositionManager?.Positions?
                .Where(x => x.Record != default && x.Record.StudentKey != default)
                .Select(x=>x.Record.DisciplineKey); // Select keys from all signed up students for cases manually signed up students

            var discipline = (await context.Education.GetDisciplinesByKeys(disciplineKeys)).ToList();

            var studentsKeys = reception.PositionManager?.Positions?
                .Where(x => x.Record != default && x.Record.StudentKey != default)
                .Select(x => x.Record.StudentKey);

            var students = (await context.Student.GetStudents(studentsKeys)).ToList();

            //var persons = (await context.Person.GetByStudent(studentsKeys)).ToList();

            var programsKeys = reception.PositionManager?.Positions?
                .Where(x => x.Record != default && x.Record.StudentKey != default && x.Record.ProgramKey != default)
                .Select(x => x.Record.ProgramKey);

            var programs = (await context.Education.GetProgramsByKeys(programsKeys)).ToList();

            var controlTypeKeys = programs.SelectMany(x=>x.Educations.Select(c=>c.ControlType.Key)).Where(x=>x != default).Distinct();

            var controlTypes = await context.Education.GetControlTypesByKeys(controlTypeKeys);

            var rates = await context.Education.GetRates();

            var viewModel = new TableViewModel(reception).IncludePositions(students,programs,discipline,controlTypes,rates);

            logger.LogInformation("Получена рецепция-табель");
            logger.LogDebug("Полученны данные - {@Result}", viewModel);

            if (viewModel == default) return NoContent();

            return viewModel;
        }

    }
}
