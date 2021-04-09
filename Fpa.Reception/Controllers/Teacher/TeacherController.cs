using Application.Extensions;
using Domain;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;
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
        /// <param name="employeeKey">Method takes a key of employee</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEducation")]
        public async Task<ActionResult<IEnumerable<Domain.Education.Program>>> GetEducationByEmployeeKey(Guid employeeKey)
        {
            var programs = await context.Teacher.GetEducation(employeeKey);
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
            // Get Reception
            // Get Discipline Title
            // Get Students person Title
            // Get Students Program Title


            var reception = context.Reception.Get(key);
            if (reception == default) return BadRequest(nameof(key));

            var discipline = (await context.Education.GetDisciplinesByKeys(reception.Events.Select(x=>x.Discipline.Key))).ToList();

            var studentsKeys = reception.PositionManager.Positions
                .Where(x => x.Record != default && x.Record.StudentKey != default)
                .Select(x => x.Record.StudentKey);

            var students = (await context.Student.GetByKeys(studentsKeys)).ToList();

            var persons = (await context.Person.GetByStudent(studentsKeys)).ToList();

            var programsKeys = reception.PositionManager.Positions
                .Where(x => x.Record != default && x.Record.StudentKey != default && x.Record.ProgramKey != default)
                .Select(x => x.Record.ProgramKey);

            var programs = (await context.Education.GetProgramsByKeys(programsKeys)).ToList();

            var vm = new {
                Discipline = discipline.FirstOrDefault(),
                Date = reception.Date,
                Position = reception.PositionManager.Positions
                                                    .Where(x=>x != default && x.Record != default)
                                                    .Select(x => GetTimeViewModel(x))
            };

            return vm;

            dynamic GetTimeViewModel(Position position)
            {
                var student = students.FirstOrDefault(x => x.Key == position.Record.StudentKey);
                var person = persons.FirstOrDefault(x => x.Key == student.Owner);

                var program = programs.FirstOrDefault(x => x.Key == position.Record.ProgramKey);

                return new
                {
                    Time = position.Time,
                    Student = new { Title = person.Title, Key = student.Key },
                    Program = new { Title = program.Title, Key = program.Key },
                    PositionKey = position.Key
                };
            }
        }

    }
}
