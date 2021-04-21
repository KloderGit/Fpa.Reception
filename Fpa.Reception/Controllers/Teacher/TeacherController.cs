using Application.Extensions;
using Domain;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using reception.fitnesspro.ru.Controllers.Teacher.ViewModel;
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

        public TeacherController(IAppContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Get information in which programs and disciplines the teacher is invoved in
        /// </summary>
        /// <param name="key">Method takes a key of employee</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEducation")]
        public async Task<ActionResult<IEnumerable<Domain.Education.Program>>> GetEducationByEmployeeKey(Guid key)
        {
            var programs = await context.Teacher.GetEducation(key);
            if (programs.IsNullOrEmpty()) return NoContent();

            return programs.ToList();
        }

        [HttpGet]
        [Route("GetSchedule")]
        public async Task<ActionResult<IEnumerable<Domain.Reception>>> GetScheduleFromReceptions(Guid employeeKey, Guid disciplineKey, DateTime fromDate, DateTime toDate)
        {
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            if(fromDate == default) fromDate = new DateTime(currentYear, currentMonth, 1);
            if(toDate == default) toDate = new DateTime(currentYear, currentMonth, DateTime.DaysInMonth(currentYear,currentMonth));
            if(toDate > fromDate)
            { 
                var temp = toDate;
                toDate = fromDate;
                fromDate = temp;
            }

            var receptions = await context.Teacher.GetReceptions(employeeKey, disciplineKey, fromDate, toDate);

            return receptions.ToList();
        }


        [HttpGet]
        [Route("GetTable")]
        public async Task<dynamic> GetTableFromReception([FromQuery] Guid key)
        {
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

            return viewModel;
        }

    }
}
